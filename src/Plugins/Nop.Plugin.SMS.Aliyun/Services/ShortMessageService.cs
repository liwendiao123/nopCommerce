using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Core.Plugins;
using Nop.Plugin.SMS.Aliyun.Constants;
using Nop.Plugin.SMS.Aliyun.Domain;
using Nop.Plugin.SMS.Aliyun.Extensions;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nop.Plugin.SMS.Aliyun.Services
{
    public partial class ShortMessageService : IShortMessageService
    {
        #region Constants

        private const string TOKEN_NOP_PATTERN = @"(%([\w.]+)%)";
        private const string TOKEN_ALIYUN_PATTERN = @"(\$\{(\w+)\})";
        private const string TOKEN_TEST_VALUE = @"XXX";

        #endregion

        #region Fields

        //private readonly ICacheManager _cacheManager;
        private readonly ISmsTemplateService _smsTemplateService;
        private readonly IQueuedSmsService _queuedSmsService;
        private readonly ILocalizationService _localizationService;
        private readonly IJsonTokenizer _tokenizer;
        private readonly IShortMessageTokenProvider _messageTokenProvider;
        private readonly ILanguageService _languageService;
        private readonly IStoreService _storeService;
        private readonly IStoreContext _storeContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IPluginFinder _pluginFinder;

        #endregion

        #region Ctor

        public ShortMessageService(ISmsTemplateService smsTemplateService,
            IQueuedSmsService queuedSmsService,
            ILocalizationService localizationService,
            IJsonTokenizer tokenizer,
            IShortMessageTokenProvider messageTokenProvider,
            ILanguageService languageService,
            IStoreService storeService,
            IStoreContext storeContext,
            IEventPublisher eventPublisher,
            IPluginFinder pluginFinder)
        {
            this._smsTemplateService = smsTemplateService;
            this._queuedSmsService = queuedSmsService;
            this._localizationService = localizationService;
            this._languageService = languageService;
            this._messageTokenProvider = messageTokenProvider;
            _storeService = storeService;
            _storeContext = storeContext;
            _eventPublisher = eventPublisher;
            this._tokenizer = tokenizer;
            this._pluginFinder = pluginFinder;
        }

        #endregion

        #region Utilities

        private static string GetFirstMatch(string pattern, string originalText)
        {
            MatchCollection matches = GetMatches(pattern, originalText);
            if (matches.Count > 0)
            {
                return matches[0].Value;
            }
            else
            {
                return null;
            }
        }

        private static MatchCollection GetMatches(string pattern, string originalText)
        {
            Regex tmpRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = tmpRegex.Matches(originalText);

            return matches;
        }


        /// <summary>
        /// Get message template
        /// </summary>
        /// <param name="messageTemplateName">Message template name</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Message template</returns>
        protected virtual SmsTemplate GetActiveMessageTemplate(string messageTemplateSystem, int storeId)
        {
            var messageTemplate = _smsTemplateService.GetSmsTemplateBySystemName(messageTemplateSystem, storeId);

            //no template found
            if (messageTemplate == null)
                return null;

            //ensure it's active
            var isActive = messageTemplate.IsActive;
            if (!isActive)
                return null;

            return messageTemplate;
        }

        /// <summary>
        /// Ensure language is active
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Return a value language identifier</returns>
        protected virtual int EnsureLanguageIsActive(int languageId, int storeId)
        {
            //load language by specified ID
            var language = _languageService.GetLanguageById(languageId);

            if (language == null || !language.Published)
            {
                //load any language from the specified store
                language = _languageService.GetAllLanguages(storeId: storeId).FirstOrDefault();
            }
            if (language == null || !language.Published)
            {
                //load any language
                language = _languageService.GetAllLanguages().FirstOrDefault();
            }

            if (language == null)
                throw new Exception("No active language could be loaded");
            return language.Id;
        }

        #endregion

        #region Methods

        public virtual SendShortMessageResult SendNotification(SmsTemplate messageTemplate,
            IEnumerable<Token> tokens,
            string toMobileNumber, string toName, int languageId,
            string subject = null)
        {
            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            //retrieve localized message template data
            if (string.IsNullOrEmpty(subject))
                subject = messageTemplate.GetLocalized(mt => mt.Subject, languageId);
            var body = messageTemplate.GetLocalized(mt => mt.Body, languageId);

            //Replace subject and body tokens 
            var subjectReplaced = _tokenizer.Replace(subject, tokens, false);
            var bodyReplaced = _tokenizer.Replace(body, tokens, true);

            //limit name length
            toName = CommonHelper.EnsureMaximumLength(toName, 300);

            var sm = new QueuedSms
            {
                Priority = QueuedSmsPriority.High,
                ToMobileNumber = toMobileNumber,
                ToName = toName,
                TemplateCode= messageTemplate.TemplateCode,
                Subject = subjectReplaced,
                Body = bodyReplaced,
                OutGuid = Guid.NewGuid(),
                TemplateParamJson = tokens.ToJson(),
                CreatedOnUtc = DateTime.UtcNow,
                DontSendBeforeDateUtc = !messageTemplate.DelayBeforeSend.HasValue ? null
                    : (DateTime?)(DateTime.UtcNow + TimeSpan.FromHours(messageTemplate.DelayPeriod.ToHours(messageTemplate.DelayBeforeSend.Value)))
            };
            // 立即发送的短信，如果发送不成功， 不写到数据库（也就是期望重新发送一次）
            if (!messageTemplate.DelayBeforeSend.HasValue)
            {
                var smsSenders = _pluginFinder.GetPlugins<ISmsSender>().ToList();
                if (smsSenders.Count > 0)
                {
                    //var smsSender = EngineContext.Current.Resolve<ISmsSender>();
                    var descriptor = _pluginFinder.GetPluginDescriptorBySystemName<ISmsSender>("SMS.Aliyun");
                    if (descriptor != null)
                    {
                        var smsSender = descriptor.Instance<ISmsSender>(); // EngineContext.Current.Resolve<ISmsSender>();
                        if (smsSender != null)
                        {
                            //smsSender.SendSms(sms.ToMobile, sms.Body);
                            var sent = smsSender.SendSms(phoneNumbers: sm.ToMobileNumber,
                                templateCode: messageTemplate.TemplateCode,
                                templateParams: sm.TemplateParamJson,
                                outId: sm.OutGuid.ToString());
                            if (sent)
                            {
                                sm.SentOnUtc = DateTime.UtcNow;
                                sm.SentTries = sm.SentTries + 1;
                                _queuedSmsService.InsertQueuedSms(sm);
                                return new SendShortMessageResult(sm.Id, sm.Body);
                            }
                            else
                            {
                                var result = new SendShortMessageResult();
                                result.AddError(_localizationService.GetResource("Plugins.SMS.Aliyun.Test.Failure"));//"发送短信失败，请检查日志"
                            }
                        }
                    }
                }
            }
            else
            {
                _queuedSmsService.InsertQueuedSms(sm);
            }

            return new SendShortMessageResult(sm.Id, sm.Body);
        }


        public virtual SendShortMessageResult SendCustomerRegisteredShortMessage(Customer customer, int languageId)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var store = _storeContext.CurrentStore;
            languageId = EnsureLanguageIsActive(languageId, store.Id);

            var result = new SendShortMessageResult();

            var messageTemplate = GetActiveMessageTemplate(SmsTemplateSystemNames.CustomerRegisteredNotification, store.Id);
            if (messageTemplate == null)
            {
                result.AddError("没有找到短信模板");
                return result;
            }

            //tokens
            var tokens = new List<Token>();
            _messageTokenProvider.AddStoreTokens(tokens, store);
            _messageTokenProvider.AddCustomerTokens(tokens, customer);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

            var phone = customer.GetAttribute<string>(SystemCustomerAttributeNames.Phone);

            return SendNotification(messageTemplate, tokens, phone, customer.GetFullName(), languageId);
        }

        public virtual SendShortMessageResult SendTestShortMessage(SmsTemplate template, string sendTo, int languageId)
        {
            if (template == null)
                throw new ArgumentException("Template cannot be loaded");

            var tokens = new List<Token>();
            // var form = model.Form;
            var matches = GetMatches(TOKEN_ALIYUN_PATTERN, template.Body);
            if (matches == null || matches.Count == 0)
            {
                matches = GetMatches(TOKEN_NOP_PATTERN, template.Body);
            }
            foreach (var m in matches)
            {
                var match = m as Match;
                if (match != null)
                {
                    foreach (var g in match.Groups)
                    {
                        var group = g as Group;
                        if (group != null && !group.Value.StartsWith("$") && tokens.Count(t => t.Key == group.Value) == 0)
                        {
                            tokens.Add(new Token(group.Value, TOKEN_TEST_VALUE));
                        }
                    }
                }
            }

            //event notification
            _eventPublisher.MessageTokensAdded(template, tokens);

            return SendNotification(template, tokens, sendTo, "test user", languageId, "Test Short Message");
        }

        //public virtual SendShortMessageResult SendTestShortMessage(int messageTemplateId, string sendTo, List<Token> tokens, int languageId)
        //{
        //    var messageTemplate = _smsTemplateService.GetSmsTemplateById(messageTemplateId);
        //    if (messageTemplate == null)
        //        throw new ArgumentException("Template cannot be loaded");

        //    //event notification
        //    _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

        //    return SendNotification(messageTemplate,  tokens, sendTo, "test user", languageId, "Test Short Message");
        //}

        public SendShortMessageResult SendOrderPaidStoreOwnerNotification(Order order, int languageId)
        {
            throw new NotImplementedException();
        }

        public SendShortMessageResult SendShipmentSentCustomerNotification(Shipment shipment, int languageId)
        {
            throw new NotImplementedException();
        }

        public SendShortMessageResult SendShipmentDeliveredCustomerNotification(Shipment shipment, int languageId)
        {
            throw new NotImplementedException();
        }

        public SendShortMessageResult SendNewReturnRequestCustomerNotification(ReturnRequest returnRequest, OrderItem orderItem, int languageId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
