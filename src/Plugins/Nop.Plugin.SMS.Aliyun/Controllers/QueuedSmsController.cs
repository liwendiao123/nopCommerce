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
using Nop.Services.Logging;
using Nop.Plugin.SMS.Aliyun.Constants;
using Nop.Services.Helpers;
using Nop.Core;

namespace Nop.Plugin.SMS.Aliyun.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public partial class QueuedSmsController : BasePluginController
    {
        #region Fields

        private readonly IQueuedSmsService _queuedSmsService;
        //private readonly ICountryService _countryService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        //private readonly IStateProvinceService _stateProvinceService;
        //private readonly IStorePickupPointService _storePickupPointService;
        //private readonly IStoreService _storeService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public QueuedSmsController(IQueuedSmsService queuedSmsService,
            //ICountryService countryService,
            ILanguageService languageService,
            ILocalizedEntityService localizedEntityService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            ICustomerActivityService customerActivityService,
            IDateTimeHelper dateTimeHelper,
         
            IWorkContext workContext)
        //IStateProvinceService stateProvinceService,
        //IStorePickupPointService storePickupPointService,
        //IStoreService storeService)
        {
            this._queuedSmsService = queuedSmsService;
            //this._countryService = countryService;
            this._languageService = languageService;
            this._localizedEntityService = localizedEntityService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            //this._stateProvinceService = stateProvinceService;
            //this._storePickupPointService = storePickupPointService;
            //this._storeService = storeService;
            this._customerActivityService = customerActivityService;
            this._dateTimeHelper = dateTimeHelper;
         
            this._workContext = workContext;
        }

        #endregion

        #region Utilities

        


        protected virtual void PrepareModel(QueuedSmsModel model, QueuedSms queuedSms, bool excludeProperties = false)
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

            var model = new QueuedSmsListModel
            {
                //default value
                SearchMaxSentTries = 10
            };

            return View(ViewNames.QueuedSmsList, model);
            // return View("~/Plugins/Pickup.PickupInStore/Views/Configure.cshtml");
        }

        [HttpPost]
        [AdminAntiForgery]
        public IActionResult List(DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedKendoGridJson();

            var templates = _queuedSmsService.GetAll(pageIndex: command.Page - 1, pageSize: command.PageSize);
            var model = templates.Select(x =>
            {
                var sm = x.ToModel();
                sm.PriorityName = x.Priority.GetLocalizedEnum(_localizationService, _workContext);
                sm.CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc);
                if (x.DontSendBeforeDateUtc.HasValue)
                    sm.DontSendBeforeDate = _dateTimeHelper.ConvertToUserTime(x.DontSendBeforeDateUtc.Value, DateTimeKind.Utc);
                if (x.SentOnUtc.HasValue)
                    sm.SentOn = _dateTimeHelper.ConvertToUserTime(x.SentOnUtc.Value, DateTimeKind.Utc);

                //little performance optimization: ensure that "Body" is not returned
                sm.Body = "";

                return sm;
            }).ToList();

            return Json(new DataSourceResult
            {
                Data = model,
                Total = templates.TotalCount
            });
        }

        public IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedView();

            var messageTemplate = _queuedSmsService.GetById(id);
            if (messageTemplate == null)
                //no message template found with the specified id
                return RedirectToAction("List");

            var model = messageTemplate.ToModel();
            //model.SendImmediately = !model.DelayBeforeSend.HasValue;
            //model.HasAttachedDownload = model.AttachedDownloadId > 0;
            //var allowedTokens = string.Join(", ", _messageTokenProvider.GetListOfAllowedTokens(messageTemplate.GetTokenGroups()));
            //model.AllowedTokens = $"{allowedTokens}{Environment.NewLine}{Environment.NewLine}{_localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Tokens.ConditionalStatement")}{Environment.NewLine}";

            ////available email accounts
            //foreach (var ea in _emailAccountService.GetAllEmailAccounts())
            //    model.AvailableEmailAccounts.Add(new SelectListItem { Text = ea.DisplayName, Value = ea.Id.ToString() });

            //return View("~/Plugins/Pickup.PickupInStore/Views/Edit.cshtml", model);
            return View(ViewNames.QueuedSmsEdit, model);
        }

        [AdminAntiForgery]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public IActionResult Edit(QueuedSmsModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedView();

            var messageTemplate = _queuedSmsService.GetById(model.Id);
            if (messageTemplate == null)
                //no message template found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                messageTemplate = model.ToEntity(messageTemplate);

                //attached file
                //if (!model.HasAttachedDownload)
                //    messageTemplate.AttachedDownloadId = 0;
                //if (model.SendImmediately)
                //    messageTemplate.DelayBeforeSend = null;
                _queuedSmsService.UpdateQueuedSms(messageTemplate);

                //activity log
                _customerActivityService.InsertActivity("EditMessageTemplate", _localizationService.GetResource("ActivityLog.EditMessageTemplate"), messageTemplate.Id);

                ////stores
                //SaveStoreMappings(messageTemplate, model);

                ////locales
                //UpdateLocales(messageTemplate, model);

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
            //PrepareStoresMappingModel(model, messageTemplate, true);

            //return View("~/Plugins/Pickup.PickupInStore/Views/Edit.cshtml", model);
            return View(ViewNames.QueuedSmsEdit, model);
        }

        [HttpPost]
        [AdminAntiForgery]
        public IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedView();

            var messageTemplate = _queuedSmsService.GetById(id);
            if (messageTemplate == null)
                //no message template found with the specified id
                return RedirectToAction("List");

            _queuedSmsService.DeleteQueuedSms(messageTemplate);

            //activity log
            _customerActivityService.InsertActivity("DeleteMessageTemplate", _localizationService.GetResource("ActivityLog.DeleteMessageTemplate"), messageTemplate.Id);

            SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Deleted"));
            return RedirectToAction("List");
        }

        #endregion
    }
}
