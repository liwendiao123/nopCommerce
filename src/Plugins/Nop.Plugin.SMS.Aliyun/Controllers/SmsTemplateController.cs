using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.SMS.Aliyun.Services;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.SMS.Aliyun.Extensions;
using Nop.Plugin.SMS.Aliyun.Models;
using Nop.Plugin.SMS.Aliyun.Domain;
using Nop.Services.Stores;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Services.Logging;
using Nop.Plugin.SMS.Aliyun.Constants;
using Nop.Services.Messages;
using System.Text.RegularExpressions;

namespace Nop.Plugin.SMS.Aliyun.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public partial class SmsTemplateController : BasePluginController
    {
        #region Fields

        private readonly ISmsTemplateService _smsTemplateService;
        //private readonly ICountryService _countryService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IStoreService _storeService;
        private readonly IStoreMappingService _storeMappingService;
        //private readonly IStoreService _storeService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IShortMessageService _workflowMessageService;

        #endregion

        #region Ctor

        public SmsTemplateController(ISmsTemplateService smsTemplateService,
            //ICountryService countryService,
            ILanguageService languageService,
            ILocalizedEntityService localizedEntityService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IStoreMappingService storeMappingService,
            IPermissionService permissionService,
            IShortMessageService workflowMessageService,
            ICustomerActivityService customerActivityService)
            //IStateProvinceService stateProvinceService,
            //IStorePickupPointService storePickupPointService,
            //IStoreService storeService)
        {
            this._smsTemplateService = smsTemplateService;
            //this._countryService = countryService;
            this._languageService = languageService;
            this._localizedEntityService = localizedEntityService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            //this._stateProvinceService = stateProvinceService;
            //this._storePickupPointService = storePickupPointService;
            //this._storeService = storeService;
            this._storeService = storeService;
            this._storeMappingService = storeMappingService;

            this._customerActivityService = customerActivityService;
            this._workflowMessageService = workflowMessageService;
        }

        #endregion

        #region Utilities

        protected virtual void UpdateLocales(SmsTemplate mt, SmsTemplateModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(mt,
                    x => x.Subject,
                    localized.Subject,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(mt,
                    x => x.Body,
                    localized.Body,
                    localized.LanguageId);
            }
        }

        protected virtual void PrepareStoresMappingModel(SmsTemplateModel model, SmsTemplate messageTemplate, bool excludeProperties)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!excludeProperties && messageTemplate != null)
                model.SelectedStoreIds = _storeMappingService.GetStoresIdsWithAccess(messageTemplate).ToList();

            var allStores = _storeService.GetAllStores();
            foreach (var store in allStores)
            {
                model.AvailableStores.Add(new SelectListItem
                {
                    Text = store.Name,
                    Value = store.Id.ToString(),
                    Selected = model.SelectedStoreIds.Contains(store.Id)
                });
            }
        }

        protected virtual void SaveStoreMappings(SmsTemplate messageTemplate, SmsTemplateModel model)
        {
            messageTemplate.LimitedToStores = model.SelectedStoreIds.Any();

            var existingStoreMappings = _storeMappingService.GetStoreMappings(messageTemplate);
            var allStores = _storeService.GetAllStores();
            foreach (var store in allStores)
            {
                if (model.SelectedStoreIds.Contains(store.Id))
                {
                    //new store
                    if (existingStoreMappings.Count(sm => sm.StoreId == store.Id) == 0)
                        _storeMappingService.InsertStoreMapping(messageTemplate, store.Id);
                }
                else
                {
                    //remove store
                    var storeMappingToDelete = existingStoreMappings.FirstOrDefault(sm => sm.StoreId == store.Id);
                    if (storeMappingToDelete != null)
                        _storeMappingService.DeleteStoreMapping(storeMappingToDelete);
                }
            }
        }

        protected virtual void PrepareModel(SmsTemplateModel model, SmsTemplate template, bool excludeProperties = false)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            //if (!excludeProperties && template != null)
            //    model.SelectedDiscountIds = manufacturer.AppliedDiscounts.Select(d => d.Id).ToList();

            //foreach (var discount in _discountService.GetAllDiscounts(DiscountType.AssignedToManufacturers, showHidden: true))
            //{
            //    model.AvailableDiscounts.Add(new SelectListItem
            //    {
            //        Text = discount.Name,
            //        Value = discount.Id.ToString(),
            //        Selected = model.SelectedDiscountIds.Contains(discount.Id)
            //    });
            //}
        }

        #endregion


        #region Methods

        public IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedView();

            var model = new SmsTemplateListModel();
            //stores
            model.AvailableStores.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var s in _storeService.GetAllStores())
                model.AvailableStores.Add(new SelectListItem { Text = s.Name, Value = s.Id.ToString() });

            return View(ViewNames.SmsTemplateList, model);
        }

        [HttpPost]
        [AdminAntiForgery]
        public IActionResult List(DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedKendoGridJson();

            var templates = _smsTemplateService.GetAll(pageIndex: command.Page - 1, pageSize: command.PageSize);
            var model = templates.Select(x => x.ToModel())
                .ToList();

            return Json(new DataSourceResult
            {
                Data = model,
                Total = templates.TotalCount
            });
        }

        public IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            var model = new SmsTemplateModel();
            PrepareModel(model, null);
            //locales
            AddLocales(_languageService, model.Locales);

            return View(ViewNames.SmsTemplateCreate, model);
            //return View("~/Plugins/SMS.Aliyun/Views/SmsTemplate/Create.cshtml", model);
        }

        [AdminAntiForgery]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Create(SmsTemplateModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var messageTemplate = model.ToEntity();

                if (model.SendImmediately)
                    messageTemplate.DelayBeforeSend = null;
                _smsTemplateService.InsertSmsTemplate(messageTemplate);

                //activity log
                _customerActivityService.InsertActivity("EditMessageTemplate", _localizationService.GetResource("ActivityLog.EditMessageTemplate"), messageTemplate.Id);

                //stores
                SaveStoreMappings(messageTemplate, model);

                //locales
                UpdateLocales(messageTemplate, model);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Updated"));

                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = messageTemplate.Id });
                }
                return RedirectToAction("List");
            }

            ////if we got this far, something failed, redisplay form
            //model.HasAttachedDownload = model.AttachedDownloadId > 0;
            //var allowedTokens = string.Join(", ", _messageTokenProvider.GetListOfAllowedTokens(messageTemplate.GetTokenGroups()));
            //model.AllowedTokens = $"{allowedTokens}{Environment.NewLine}{Environment.NewLine}{_localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Tokens.ConditionalStatement")}{Environment.NewLine}";

            ////available email accounts
            //foreach (var ea in _emailAccountService.GetAllEmailAccounts())
            //    model.AvailableEmailAccounts.Add(new SelectListItem { Text = ea.DisplayName, Value = ea.Id.ToString() });

            ////locales (update email account dropdownlists)
            //foreach (var locale in model.Locales)
            //{

            //}

            //store
            PrepareStoresMappingModel(model, null, true);
            //return View(model);
            return View(ViewNames.SmsTemplateCreate, model);
            //return View("~/Plugins/Pickup.PickupInStore/Views/Create.cshtml", model);
        }

        public IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedView();

            var messageTemplate = _smsTemplateService.GetSmsTemplateById(id);
            if (messageTemplate == null)
                //no message template found with the specified id
                return RedirectToAction("List");

            var model = messageTemplate.ToModel();
            model.SendImmediately = !model.DelayBeforeSend.HasValue;
            //model.HasAttachedDownload = model.AttachedDownloadId > 0;
            //var allowedTokens = string.Join(", ", _messageTokenProvider.GetListOfAllowedTokens(messageTemplate.GetTokenGroups()));
            //model.AllowedTokens = $"{allowedTokens}{Environment.NewLine}{Environment.NewLine}{_localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Tokens.ConditionalStatement")}{Environment.NewLine}";

            ////available email accounts
            //foreach (var ea in _emailAccountService.GetAllEmailAccounts())
            //    model.AvailableEmailAccounts.Add(new SelectListItem { Text = ea.DisplayName, Value = ea.Id.ToString() });

            //store
            PrepareStoresMappingModel(model, messageTemplate, false);

            //locales
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                //locale.BccEmailAddresses = messageTemplate.GetLocalized(x => x.BccEmailAddresses, languageId, false, false);
                locale.Subject = messageTemplate.GetLocalized(x => x.Subject, languageId, false, false);
                locale.Body = messageTemplate.GetLocalized(x => x.Body, languageId, false, false);
                //locale.EmailAccountId = messageTemplate.GetLocalized(x => x.EmailAccountId, languageId, false, false);

                //available email accounts (we add "Standard" value for localizable field)
                //locale.AvailableEmailAccounts.Add(new SelectListItem
                //{
                //    Text = _localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Fields.EmailAccount.Standard"),
                //    Value = "0"
                //});

                //foreach (var ea in _emailAccountService.GetAllEmailAccounts())
                //    locale.AvailableEmailAccounts.Add(new SelectListItem
                //    {
                //        Text = ea.DisplayName,
                //        Value = ea.Id.ToString(),
                //        Selected = ea.Id == locale.EmailAccountId
                //    });
            });
            return View(ViewNames.SmsTemplateEdit, model);
            //return View("~/Plugins/Pickup.PickupInStore/Views/Edit.cshtml", model);
        }

        [AdminAntiForgery]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public IActionResult Edit(SmsTemplateModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedView();

            var messageTemplate = _smsTemplateService.GetSmsTemplateById(model.Id);
            if (messageTemplate == null)
                //no message template found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                messageTemplate = model.ToEntity(messageTemplate);

                //attached file
                //if (!model.HasAttachedDownload)
                //    messageTemplate.AttachedDownloadId = 0;
                if (model.SendImmediately)
                    messageTemplate.DelayBeforeSend = null;
                _smsTemplateService.UpdateSmsTemplate(messageTemplate);

                //activity log
                _customerActivityService.InsertActivity("EditMessageTemplate", _localizationService.GetResource("ActivityLog.EditMessageTemplate"), messageTemplate.Id);

                //stores
                SaveStoreMappings(messageTemplate, model);

                //locales
                UpdateLocales(messageTemplate, model);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Updated"));

                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = messageTemplate.Id });
                }
                return RedirectToAction("List");
            }

            ////if we got this far, something failed, redisplay form
            //model.HasAttachedDownload = model.AttachedDownloadId > 0;
            //var allowedTokens = string.Join(", ", _messageTokenProvider.GetListOfAllowedTokens(messageTemplate.GetTokenGroups()));
            //model.AllowedTokens = $"{allowedTokens}{Environment.NewLine}{Environment.NewLine}{_localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Tokens.ConditionalStatement")}{Environment.NewLine}";

            ////available email accounts
            //foreach (var ea in _emailAccountService.GetAllEmailAccounts())
            //    model.AvailableEmailAccounts.Add(new SelectListItem { Text = ea.DisplayName, Value = ea.Id.ToString() });

            ////locales (update email account dropdownlists)
            //foreach (var locale in model.Locales)
            //{
            //    //available email accounts (we add "Standard" value for localizable field)
            //    locale.AvailableEmailAccounts.Add(new SelectListItem
            //    {
            //        Text = _localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Fields.EmailAccount.Standard"),
            //        Value = "0"
            //    });
            //    foreach (var ea in _emailAccountService.GetAllEmailAccounts())
            //        locale.AvailableEmailAccounts.Add(new SelectListItem
            //        {
            //            Text = ea.DisplayName,
            //            Value = ea.Id.ToString(),
            //            Selected = ea.Id == locale.EmailAccountId
            //        });
            //}

            //store
            PrepareStoresMappingModel(model, messageTemplate, true);
            return View(ViewNames.SmsTemplateEdit, model);
            //return View("~/Plugins/Pickup.PickupInStore/Views/Edit.cshtml", model);
        }

        [HttpPost]
        [AdminAntiForgery]
        public IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedView();

            var messageTemplate = _smsTemplateService.GetSmsTemplateById(id);
            if (messageTemplate == null)
                //no message template found with the specified id
                return RedirectToAction("List");

            _smsTemplateService.DeleteSmsTemplate(messageTemplate);

            //activity log
            _customerActivityService.InsertActivity("DeleteMessageTemplate", _localizationService.GetResource("ActivityLog.DeleteMessageTemplate"), messageTemplate.Id);

            SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Deleted"));
            return RedirectToAction("List");
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("message-template-copy")]
        public virtual IActionResult CopyTemplate(SmsTemplateModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedView();

            var messageTemplate = _smsTemplateService.GetSmsTemplateById(model.Id);
            if (messageTemplate == null)
                //No message template found with the specified id
                return RedirectToAction("List");

            try
            {
                var newMessageTemplate = _smsTemplateService.CopySmsTemplate(messageTemplate);
                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Copied"));
                return RedirectToAction("Edit", new { id = newMessageTemplate.Id });
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = model.Id });
            }
        }

        //public virtual IActionResult TestTemplate(int id, int languageId = 0)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
        //        return AccessDeniedView();

        //    var messageTemplate = _smsTemplateService.GetById(id);
        //    if (messageTemplate == null)
        //        //No message template found with the specified id
        //        return RedirectToAction("List");

        //    var model = new TestSmsTemplateModel
        //    {
        //        Id = messageTemplate.Id,
        //        LanguageId = languageId
        //    };

        //    //filter tokens to the current template
        //    var subject = messageTemplate.GetLocalized(mt => mt.Subject, languageId);
        //    var body = messageTemplate.GetLocalized(mt => mt.Body, languageId);
        //    model.Tokens = _messageTokenProvider.GetListOfAllowedTokens().Where(x => subject.Contains(x) || body.Contains(x)).ToList();

        //    return View(model);
        //}

        //[HttpPost, ActionName("TestTemplate")]
        //[FormValueRequired("send-test")]
        //public virtual IActionResult TestTemplate(TestSmsTemplateModel model)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
        //        return AccessDeniedView();

        //    var messageTemplate = _smsTemplateService.GetById(model.Id);
        //    if (messageTemplate == null)
        //        //No message template found with the specified id
        //        return RedirectToAction("List");

        //    var tokens = new List<Token>();
        //    var form = model.Form;
        //    foreach (var formKey in form.Keys)
        //        if (formKey.StartsWith("token_", StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            var tokenKey = formKey.Substring("token_".Length).Replace("%", "");
        //            var stringValue = form[formKey];

        //            //try get non-string value
        //            object tokenValue;
        //            if (bool.TryParse(stringValue, out bool boolValue))
        //                tokenValue = boolValue;
        //            else if (int.TryParse(stringValue, out int intValue))
        //                tokenValue = intValue;
        //            else if (decimal.TryParse(stringValue, out decimal decimalValue))
        //                tokenValue = decimalValue;
        //            else
        //                tokenValue = stringValue;

        //            tokens.Add(new Token(tokenKey, tokenValue));
        //        }

        //    _workflowMessageService.SendTestShortMessage(messageTemplate.Id, model.SendTo, tokens, model.LanguageId);

        //    if (ModelState.IsValid)
        //    {
        //        SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Test.Success"));
        //    }

        //    return RedirectToAction("Edit", new { id = messageTemplate.Id });
        //}

        [HttpPost]
        public IActionResult TestTemplate(TestSmsTemplateModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                //return Content("Access denied: No Authorize");
                return Json(new { success = false, message = _localizationService.GetResource("Admin.Common.NoAuthorize") });

            var messageTemplate = _smsTemplateService.GetSmsTemplateById(model.Id);
            if (messageTemplate == null)
                //No message template found with the specified id
                //return RedirectToAction("List");
                return Json(new { success = false, message = _localizationService.GetResource("Admin.SmsTemplates.NoExisted") });

            // tokens.Add(new Token(tokenKey, tokenValue));
            var result = _workflowMessageService.SendTestShortMessage(messageTemplate, model.SendTo, model.LanguageId);

            //return Json(new { Result = true });
            return Json(new { success = result.Success, message = result.Success? "测试短信发送成功": String.Join(";", result.Errors)  });
        }



        #endregion

    }
}
