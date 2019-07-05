using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Directory;
using Nop.Plugin.SMS.Aliyun.Domain;
using Nop.Plugin.SMS.Aliyun.Models;
using Nop.Plugin.SMS.Aliyun.Services;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.SMS.Aliyun.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public class AliyunSmsController : BasePluginController
    {
        #region Fields

        private readonly CurrencySettings _currencySettings;
        private readonly AliyunSmsSettings _aliyunSmsSettings;
        private readonly ICountryService _countryService;
        private readonly ICurrencyService _currencyService;
        private readonly ILocalizationService _localizationService;
        private readonly IMeasureService _measureService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IQueuedSmsService _queuedSmsService;
        private readonly IShippingService _shippingService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IStoreService _storeService;
        private readonly MeasureSettings _measureSettings;

        #endregion

        #region Ctor

        public AliyunSmsController(CurrencySettings currencySettings,
            AliyunSmsSettings fixedOrByWeightSettings,
            ICountryService countryService,
            ICurrencyService currencyService,
            ILocalizationService localizationService,
            IMeasureService measureService,
            IPermissionService permissionService,
            ISettingService settingService,
            IQueuedSmsService shippingByWeightService,
            IShippingService shippingService,
            IStateProvinceService stateProvinceService,
            IStoreService storeService,
            MeasureSettings measureSettings)
        {
            this._currencySettings = currencySettings;
            this._aliyunSmsSettings = fixedOrByWeightSettings;
            this._countryService = countryService;
            this._currencyService = currencyService;
            this._localizationService = localizationService;
            this._measureService = measureService;
            this._permissionService = permissionService;
            this._settingService = settingService;
            this._queuedSmsService = shippingByWeightService;
            this._stateProvinceService = stateProvinceService;
            this._shippingService = shippingService;
            this._storeService = storeService;
            this._measureSettings = measureSettings;
        }

        #endregion

        #region Plugin

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            var model = new ConfigurationModel
            {
                Enabled = _aliyunSmsSettings.Enabled,
                AccessKeyID = _aliyunSmsSettings.AccessKeyID,
                AccessKeySecret = _aliyunSmsSettings.AccessKeySecret,
                SignName = _aliyunSmsSettings.SignName,
                Product = _aliyunSmsSettings.Product,
                Domain = _aliyunSmsSettings.Domain,
                RegionId = _aliyunSmsSettings.RegionId,
                EndpointName = _aliyunSmsSettings.EndpointName
            };

            return View("~/Plugins/SMS.Aliyun/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAntiForgery]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return Content("Access denied");

            //save settings
            _aliyunSmsSettings.Enabled = model.Enabled;
            _aliyunSmsSettings.AccessKeyID = model.AccessKeyID;
            _aliyunSmsSettings.AccessKeySecret = model.AccessKeySecret;
            _aliyunSmsSettings.SignName = model.SignName;
            _aliyunSmsSettings.Product = model.Product;
            _aliyunSmsSettings.Domain = model.Domain;
            _aliyunSmsSettings.RegionId = model.RegionId;
            _aliyunSmsSettings.EndpointName = model.EndpointName;

            _settingService.SaveSetting(_aliyunSmsSettings);

            //return Json(new { Result = true });
            return Configure();
        }

        #endregion

    }
}