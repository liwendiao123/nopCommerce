using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nop.Core;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Payments;
using Nop.Plugin.Payments.Test.Models;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Senparc.Weixin.Entities;
using Senparc.Weixin.TenPay.V3;

namespace Nop.Plugin.Payments.Test.Controllers
{
   public class PaymentWeixinPayController : BasePaymentController
    {
        #region Fields
        private readonly ISettingService _settingService;
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly ILogger _logger;
        private readonly ILocalizationService _localizationService;
        private readonly WeixinPayPaymentSettings _weixinPayPaymentSettings;
        private readonly PaymentSettings _paymentSettings;
        private readonly IWebHelper _webHelper;
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly IOptions<SenparcWeixinSetting> _senparcWeixinSettings;
        private          IConfiguration _configuration;
        private static   TenPayV3Info _tenPayV3Info;
        private readonly IPermissionService _permissionService;
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INotificationService _notificationService;
        private readonly IPaymentPluginManager _paymentPluginManager;
        #endregion


        #region Ctor
        public PaymentWeixinPayController( ISettingService settingService
                                          ,IPaymentService paymentService
                                          ,IOrderService orderService
                                          ,IOrderProcessingService orderProcessingService
                                          ,ILogger logger
                                          ,ILocalizationService localizationService
                                          ,WeixinPayPaymentSettings weixinPayPaymentSettings
                                          ,PaymentSettings paymentSettings
                                          ,IWebHelper webHelper
                                          ,ICurrencyService currencyService
                                          ,CurrencySettings currencySettings
                                          ,IOptions<SenparcWeixinSetting> senparcWeixinSettings
                                          ,IConfiguration configuration
                                          ,TenPayV3Info tenPayV3Info
                                          ,IPermissionService permissionService
                                          ,IHostingEnvironment env
                                          ,IHttpContextAccessor contextAccessor
                                          ,INotificationService notificationService
                                          ,IPaymentPluginManager paymentPluginManager )
        {
            _settingService = settingService;
            _paymentService = paymentService;
             _orderService = orderService;
            _orderProcessingService = orderProcessingService;
            _logger = logger;
            _localizationService = localizationService;
            _weixinPayPaymentSettings = weixinPayPaymentSettings;
             _paymentSettings = paymentSettings;
            _webHelper = webHelper;
            _currencyService = currencyService;
            _currencySettings = currencySettings;
            _senparcWeixinSettings = senparcWeixinSettings;
            _configuration = configuration;
            _tenPayV3Info = tenPayV3Info;
            _permissionService = permissionService;
            _env = env;
            _contextAccessor = contextAccessor;
            _notificationService = notificationService;
            _paymentPluginManager = paymentPluginManager;
        }
        #endregion


        #region Method



       /// <summary>
       /// 
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="certificate"></param>
       /// <param name="chain"></param>
       /// <param name="errors"></param>
       /// <returns></returns>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="tValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private static NameValueCollection ToNameValueCollection<tValue>(IDictionary<string, tValue> dictionary)
        {
            var collection = new NameValueCollection();
            foreach (var pair in dictionary)
                collection.Add(pair.Key, pair.Value.ToString());
            return collection;
        }




        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {

            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();
            var model = new ConfigurationModel
            {
                //TenPayV3_MchId = _weixinPayPaymentSettings.TenPayV3_MchId,
                //TenPayV3_Key = _weixinPayPaymentSettings.TenPayV3_Key,
                //TenPayV3_AppId = _weixinPayPaymentSettings.TenPayV3_AppId,
                //TenPayV3_AppSecret = _weixinPayPaymentSettings.TenPayV3_AppSecret,
                //TenPayV3_TenpayNotify = _weixinPayPaymentSettings.TenPayV3_TenpayNotify,
                //TenPayV3_CertPath = _weixinPayPaymentSettings.TenPayV3_CertPath,
                //TenPayV3_CertPassword = _weixinPayPaymentSettings.TenPayV3_CertPassword
            };
            return View("~/Plugins/Payments.WeixinPay/Views/PaymentWeixinPay/Configure.cshtml", model);
        }
        #endregion
    }
}
