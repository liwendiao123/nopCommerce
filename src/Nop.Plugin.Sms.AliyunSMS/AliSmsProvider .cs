using System;
using System.Collections.Generic;
using System.Text;
using Castle.Core.Logging;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;

namespace Nop.Plugin.Sms.AliyunSMS
{
    public  class AliSmsProvider:BasePlugin,IMiscPlugin
    {
        private readonly AliSendSmsSetting _aliSendSmsSetting;
        private readonly ILogger _logger;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;

        public AliSmsProvider(
            AliSendSmsSetting aliSendSmsSetting,
            ILogger logger,
            ISettingService settingService,
            IWebHelper webHelper,
            ILocalizationService localizationService
            
            )
        {
            _aliSendSmsSetting = aliSendSmsSetting;
            _logger = logger;
            _settingService = settingService;
            _webHelper = webHelper;
            _localizationService = localizationService;
        }



        public bool SendSms(string phone, string type, Customer customer = null)
        {
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetTempCode(string type)
        {
            return "";
        }

    }
}
