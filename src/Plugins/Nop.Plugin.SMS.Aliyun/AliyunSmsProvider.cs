using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Plugin.SMS.Aliyun.Data;
using Nop.Plugin.SMS.Aliyun.Services;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Tracking;
using Nop.Web.Framework;
using Nop.Web.Framework.Menu;
using System;

namespace Nop.Plugin.SMS.Aliyun
{
    /// <summary>
    /// Fixed rate or by weight shipping computation method 
    /// </summary>
    public class AliyunSmsProvider : BasePlugin, IMiscPlugin, ISmsSender, IAdminMenuPlugin
    {
        #region Fields

        private readonly AliyunSmsSettings _smsSettings;
        //private readonly IPriceCalculationService _priceCalculationService;
        //private readonly IProductAttributeParser _productAttributeParser;
        //private readonly IProductService _productService;
        private readonly ISettingService _settingService;
        //private readonly IQueuedSmsService _shippingByWeightService;
        private readonly IShippingService _shippingService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IWebHelper _webHelper;
        private readonly AliyunSmsObjectContext _objectContext;
        private readonly ILogger _logger;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public AliyunSmsProvider(AliyunSmsSettings smsSettings,
            //IPriceCalculationService priceCalculationService,
            //IProductAttributeParser productAttributeParser,
            //IProductService productService,
            ISettingService settingService,
            //IShippingByWeightService shippingByWeightService,
            //IShippingService shippingService,
            IWorkContext workContext,
            IStoreContext storeContext,
            IWebHelper webHelper,
            AliyunSmsObjectContext objectContext,
            ILocalizationService localizationService,
            ILogger logger)
        {
            this._smsSettings = smsSettings;
            this._settingService = settingService;
            //this._priceCalculationService = priceCalculationService;
            //this._productAttributeParser = productAttributeParser;
            //this._productService = productService;
            //this._settingService = settingService;
            //this._shippingByWeightService = shippingByWeightService;
            //this._shippingService = shippingService;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._webHelper = webHelper;
            this._objectContext = objectContext;
            this._logger = logger;
            this._localizationService = localizationService;
        }

        #endregion

        #region Utilities


        #endregion

        #region ISmsSender

        public bool SendSms(string phoneNumbers, string message)
        {
            throw new NotImplementedException();
        }

        public bool SendSms(string phoneNumbers, string templateCode, string templateParams = null, string outId = null)
        {
            var returnValue = false;
            //String product = "Dysmsapi";//短信API产品名称（短信产品名固定，无需修改）
            //String domain = "dysmsapi.aliyuncs.com";//短信API产品域名（接口地址固定，无需修改）
            //String accessKeyId = _smsSettings.AccessKeyID;//你的accessKeyId，参考本文档步骤2
            //String accessKeySecret = _smsSettings.AccessKeySecret;//你的accessKeySecret，参考本文档步骤2
            IClientProfile profile = DefaultProfile.GetProfile(_smsSettings.RegionId, _smsSettings.AccessKeyID, _smsSettings.AccessKeySecret);
            // IAcsClient client = new DefaultAcsClient(profile);
            // SingleSendSmsRequest request = new SingleSendSmsRequest();
            //初始化ascClient,暂时不支持多region（请勿修改）
            DefaultProfile.AddEndpoint(_smsSettings.EndpointName,
                _smsSettings.RegionId,
                _smsSettings.Product,
                _smsSettings.Domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            try
            {
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = phoneNumbers;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = _smsSettings.SignName;
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = templateCode;// "SMS_00000001";
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = templateParams;// "{\"customer\":\"123\"}";
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                request.OutId = outId;// "yourOutId";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                //System.Console.WriteLine(sendSmsResponse.Message);
                if (sendSmsResponse.Code == "OK")
                {
                    returnValue = true;
                }
                else
                {
                    _logger.Error(sendSmsResponse.Message);
                }
            }
            catch (ServerException e)
            {
                _logger.Error("短信发送失败：" + e.ErrorMessage + ", 错误类型：" + e.ErrorType + ", 错误编号：" + e.ErrorCode + ", " + e.Data, e);
                // System.Console.WriteLine("Hello World!");
                returnValue = false;
            }
            catch (ClientException e)
            {
                //System.Console.WriteLine("Hello World!");
                _logger.Error("短信发送失败：" + e.ErrorMessage + ", 错误类型：" + e.ErrorType + ", 错误编号：" + e.ErrorCode + ", " + e.Data, e);
                returnValue = false;
            }
            return returnValue;
        }

        #endregion

        #region BasePlugin

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/AliyunSms/Configure";
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //settings
            _settingService.SaveSetting(new AliyunSmsSettings() {
                Domain = "dysmsapi.aliyuncs.com",
                Enabled = true,
                EndpointName = "cn-hangzhou",
                Product = "Dysmsapi",
                RegionId = "cn-hangzhou"
            });

            //database objects
            _objectContext.Install();

            //locales
            // 
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun", "Aliyun Short Message services");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Menu.Docs.Title", "技术咨询");
            this.AddOrUpdatePluginLocaleResource("Admin.Common.NoAuthorize", "您无权执行此操作");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Test.Failure", "发送短信失败，请检查日志");
            this.AddOrUpdatePluginLocaleResource("Admin.SmsTemplates.NoExisted", "短信模板不存在，请先创建并保存相应模板");
            // 
            //Action without authorization   

            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.List", "短信模板列表");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.AddNew", "新建短信模板");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Edit", "编辑短信模板");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Copy", "复制短信模板");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.List", "短信列表");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.AddNew", "新建短信");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Edit", "编辑短信");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Copy", "复制短信");

            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.Enabled", "Enabled");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.Enabled.Hint", "Enabled");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.AccessKeyID", "AccessKeyID");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.AccessKeyID.Hint", "AccessKeyID");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.AccessKeySecret", "AccessKeySecret");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.AccessKeySecret.Hint", "AccessKeySecret");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.SignName", "SignName");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.SignName.Hint", "SignName");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.Product", "Product");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.Product.Hint", "Product");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.Domain", "Domain");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.Domain.Hint", "Domain");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.RegionId", "RegionId");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.RegionId.Hint", "RegionId");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.EndpointName", "EndpointName");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.EndpointName.Hint", "EndpointName");

            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.SystemName", "SystemName");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.SystemName.Hint", "SystemName");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.TemplateCode", "TemplateCode");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.TemplateCode.Hint", "TemplateCode");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.Subject", "Subject");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.Subject.Hint", "Subject");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.Body", "Body");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.Body.Hint", "Body");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.IsActive", "IsActive");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.IsActive.Hint", "IsActive");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.SendImmediately", "SendImmediately");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.SendImmediately.Hint", "SendImmediately");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.DelayBeforeSend", "DelayBeforeSend");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.DelayBeforeSend.Hint", "DelayBeforeSend");

            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.Priority", "Priority");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.Priority.Hint", "Priority");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.TemplateCode", "TemplateCode");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.TemplateCode.Hint", "TemplateCode");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.Subject", "Subject");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.Subject.Hint", "Subject");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.Body", "Body");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.Body.Hint", "Body");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.ToMobileNumber", "ToMobileNumber");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.ToMobileNumber.Hint", "ToMobileNumber");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.SendImmediately", "SendImmediately");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.SendImmediately.Hint", "SendImmediately");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.ToName", "ToName");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.ToName.Hint", "ToName");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.TemplateParamJson", "TemplateParamJson");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.TemplateParamJson.Hint", "TemplateParamJson");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.CreatedOn", "CreatedOn");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.CreatedOn.Hint", "CreatedOn");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.DontSendBeforeDate", "DontSendBeforeDate");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.DontSendBeforeDate.Hint", "DontSendBeforeDate");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.SentTries", "SentTries");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.SentTries.Hint", "SentTries");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.SentOn", "SentOn");
            this.AddOrUpdatePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.SentOn.Hint", "SentOn");

            base.Install();
        }
        
        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<AliyunSmsSettings>();

            //database objects
            _objectContext.Uninstall();

            //locales

            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.Menu.Docs.Title");
            this.DeletePluginLocaleResource("Admin.Common.NoAuthorize");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.Test.Failure");
            this.DeletePluginLocaleResource("Admin.SmsTemplates.NoExisted");

            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.List");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.AddNew");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Edit");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Copy");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.List");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.AddNew");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Edit");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Copy");

            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.Enabled");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.AccessKeyID");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.AccessKeySecret");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.SignName");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.Product");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.Domain");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.RegionId");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.Settings.Fields.EndpointName");

            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.SystemName");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.TemplateCode");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.Subject");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.Body");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.IsActive");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.SendImmediately");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.SmsTemplate.Fields.DelayBeforeSend");

            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.Priority");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.TemplateCode");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.Subject");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.Body");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.ToMobileNumber");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.SendImmediately");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.ToName");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.TemplateParamJson");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.CreatedOn");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.DontSendBeforeDate");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.SentTries");
            this.DeletePluginLocaleResource("Plugins.SMS.Aliyun.QueuedSms.Fields.SentOn");

            base.Uninstall();
        }

        public void ManageSiteMap(SiteMapNode rootNode)
        {
            string pluginMenuName = _localizationService.GetResource("Plugins.SMS.Aliyun.Menu.Title", languageId: _workContext.WorkingLanguage.Id, defaultValue: "Short Messages");
            string settingsMenuName = _localizationService.GetResource("Plugins.SMS.Aliyun.Menu.Settings.Title", languageId: _workContext.WorkingLanguage.Id, defaultValue: "Short Messages Settings");
            string manageTemplatesMenuName = _localizationService.GetResource("Plugins.SMS.Aliyun.Menu.Clients.Title", languageId: _workContext.WorkingLanguage.Id, defaultValue: "Template");

            var pluginMainMenu = new SiteMapNode
            {
                Title = pluginMenuName,
                Visible = true,
                SystemName = "Manage Short Message Menu",
                IconClass = "fa-genderless"
            };

            pluginMainMenu.ChildNodes.Add(new SiteMapNode
            {
                Title = settingsMenuName,
                Url = _webHelper.GetStoreLocation() + AreaNames.Admin + "/AliyunSms/Configure",
                Visible = true,
                SystemName = "Short Message Settings",
                IconClass = "fa-genderless"
            });

            pluginMainMenu.ChildNodes.Add(new SiteMapNode
            {
                Title = manageTemplatesMenuName,
                Url = _webHelper.GetStoreLocation() + AreaNames.Admin + "/SmsTemplate/List",
                Visible = true,
                SystemName = "Manage Short Message Template",
                IconClass = "fa-genderless"
            });
            pluginMainMenu.ChildNodes.Add(new SiteMapNode
            {
                Title = _localizationService.GetResource("Plugins.SMS.Aliyun.Menu.QueuedSms.Title", languageId: _workContext.WorkingLanguage.Id, defaultValue: "Manage Short Message"),
                Url = _webHelper.GetStoreLocation() + AreaNames.Admin + "/QueuedSms/List",
                Visible = true,
                SystemName = "Manage Short Message",
                IconClass = "fa-genderless"
            });

            string pluginDocumentationUrl = "https://shop117119354.taobao.com";

            pluginMainMenu.ChildNodes.Add(new SiteMapNode
            {
                Title = _localizationService.GetResource("Plugins.SMS.Aliyun.Menu.Docs.Title"),
                Url = pluginDocumentationUrl,
                Visible = true,
                SystemName = "SMS-Aliyun-Docs-Menu",
                IconClass = "fa-genderless"
            });//TODO: target="_blank"


            rootNode.ChildNodes.Add(pluginMainMenu);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a shipping rate computation method type
        /// </summary>
        public ShippingRateComputationMethodType ShippingRateComputationMethodType
        {
            get { return ShippingRateComputationMethodType.Offline; }
        }
        
        /// <summary>
        /// Gets a shipment tracker
        /// </summary>
        public IShipmentTracker ShipmentTracker
        {
            get
            {
                //uncomment a line below to return a general shipment tracker (finds an appropriate tracker by tracking number)
                //return new GeneralShipmentTracker(EngineContext.Current.Resolve<ITypeFinder>());
                return null;
            }
        }

        #endregion
    }
}
