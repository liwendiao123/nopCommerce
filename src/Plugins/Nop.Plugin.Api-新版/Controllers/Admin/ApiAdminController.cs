﻿namespace Nop.Plugin.Api.Controllers.Admin
{
    using Microsoft.AspNetCore.Mvc;
    using Core;
    using Constants;
    using Domain;
    using MappingExtensions;
    using Models;
    using Nop.Services.Configuration;
    using Nop.Services.Localization;
    using Nop.Services.Logging;
    using Nop.Services.Stores;
    using Web.Framework;
    using Nop.Web.Framework.Controllers;
    using Web.Framework.Mvc.Filters;
    using Nop.Services.Messages;

    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public class ApiAdminController : BasePluginController
    {
        private readonly IStoreService _storeService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly ISettingService _settingService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
       private readonly INotificationService _notificationService;

        public ApiAdminController(
            IStoreService storeService,
            IStoreContext storeContext,
            IWorkContext workContext,
            ISettingService settingService,
            INotificationService notificationService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService)
        {
            _storeService = storeService;
            _storeContext = storeContext;
            _workContext = workContext;
            _settingService = settingService;
            _notificationService = notificationService;
            _customerActivityService = customerActivityService;
            _localizationService = localizationService;
        }
        
        [HttpGet]
        public ActionResult Settings()
        {

            var storeScope = _storeContext.ActiveStoreScopeConfiguration;

            var apiSettings = _settingService.LoadSetting<ApiSettings>(storeScope);

            var model = apiSettings.ToModel();

            // Store Settings
            model.ActiveStoreScopeConfiguration = storeScope;

            if (model.EnableApi_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(apiSettings, x => x.EnableApi, storeScope, false);
            if (model.AllowRequestsFromSwagger_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(apiSettings, x => x.AllowRequestsFromSwagger, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            return View(ViewNames.AdminApiSettings, model);
        }

        [HttpPost]
        public ActionResult Settings(ConfigurationModel configurationModel)
        {
            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;

            var settings = configurationModel.ToEntity();

            /* We do not clear cache after each setting update.
            * This behavior can increase performance because cached settings will not be cleared 
            * and loaded from database after each update */

            if (configurationModel.EnableApi_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(settings, x => x.EnableApi, storeScope, false);
            if (configurationModel.AllowRequestsFromSwagger_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(settings, x => x.AllowRequestsFromSwagger, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            _customerActivityService.InsertActivity("EditApiSettings", "Edit Api Settings");

            _notificationService.SuccessNotification (_localizationService.GetResource("Admin.Plugins.Saved"));

            return View(ViewNames.AdminApiSettings, configurationModel);
        }
    }
}