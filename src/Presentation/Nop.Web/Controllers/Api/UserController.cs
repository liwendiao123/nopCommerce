using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Gdpr;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Tax;
using Nop.Services.Authentication.External;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Events;
using Nop.Services.ExportImport;
using Nop.Services.Gdpr;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Tax;
using Nop.Web.Factories;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Models.Customer;

namespace Nop.Web.Controllers.Api
{
    public class UserController  : BasePublicController
    {


        private readonly AddressSettings _addressSettings;
        private readonly CaptchaSettings _captchaSettings;
        private readonly CustomerSettings _customerSettings;
        private readonly DateTimeSettings _dateTimeSettings;
        private readonly IDownloadService _downloadService;
        private readonly ForumSettings _forumSettings;
        private readonly GdprSettings _gdprSettings;
        private readonly IAddressAttributeParser _addressAttributeParser;
        private readonly IAddressModelFactory _addressModelFactory;
        private readonly IAddressService _addressService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICountryService _countryService;
        private readonly ICurrencyService _currencyService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ICustomerAttributeParser _customerAttributeParser;
        private readonly ICustomerAttributeService _customerAttributeService;
        private readonly ICustomerModelFactory _customerModelFactory;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly ICustomerService _customerService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IExportManager _exportManager;
        private readonly IExternalAuthenticationService _externalAuthenticationService;
        private readonly IGdprService _gdprService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IGiftCardService _giftCardService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IOrderService _orderService;
        private readonly IPictureService _pictureService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IStoreContext _storeContext;
        private readonly ITaxService _taxService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly MediaSettings _mediaSettings;
        private readonly StoreInformationSettings _storeInformationSettings;
        private readonly TaxSettings _taxSettings;
        public UserController(
                    AddressSettings addressSettings,
                    CaptchaSettings captchaSettings,
                    CustomerSettings customerSettings,
                    DateTimeSettings dateTimeSettings,
                    IDownloadService downloadService,
                    ForumSettings forumSettings,
                    GdprSettings gdprSettings,
                    IAddressAttributeParser addressAttributeParser,
                    IAddressModelFactory addressModelFactory,
                    IAddressService addressService,
                    IAuthenticationService authenticationService,
                    ICountryService countryService,
                    ICurrencyService currencyService,
                    ICustomerActivityService customerActivityService,
                    ICustomerAttributeParser customerAttributeParser,
                    ICustomerAttributeService customerAttributeService,
                    ICustomerModelFactory customerModelFactory,
                    ICustomerRegistrationService customerRegistrationService,
                    ICustomerService customerService,
                    IEventPublisher eventPublisher,
                    IExportManager exportManager,
                    IExternalAuthenticationService externalAuthenticationService,
                    IGdprService gdprService,
                    IGenericAttributeService genericAttributeService,
                    IGiftCardService giftCardService,
                    ILocalizationService localizationService,
                    ILogger logger,
                    INewsLetterSubscriptionService newsLetterSubscriptionService,
                    IOrderService orderService,
                    IPictureService pictureService,
                    IPriceFormatter priceFormatter,
                    IShoppingCartService shoppingCartService,
                    IStateProvinceService stateProvinceService,
                    IStoreContext storeContext,
                    ITaxService taxService,
                    IWebHelper webHelper,
                    IWorkContext workContext,
                    IWorkflowMessageService workflowMessageService,
                    LocalizationSettings localizationSettings,
                    MediaSettings mediaSettings,
                    StoreInformationSettings storeInformationSettings,
                    TaxSettings taxSettings
            )
        {
                    _addressSettings = addressSettings;
                    _captchaSettings = captchaSettings;
                    _customerSettings = customerSettings;
                    _dateTimeSettings = dateTimeSettings;
                    _downloadService = downloadService;
                    _forumSettings = forumSettings;
                    _gdprSettings = gdprSettings;
                    _addressAttributeParser = addressAttributeParser;
                    _addressModelFactory = addressModelFactory;
                    _addressService = addressService;
                    _authenticationService = authenticationService;
                    _countryService = countryService; 
                    _currencyService = currencyService;
                    _customerActivityService = customerActivityService;
                    _customerAttributeParser = customerAttributeParser;
                    _customerAttributeService = customerAttributeService;
                    _customerModelFactory = customerModelFactory;
                    _customerRegistrationService = customerRegistrationService;
                    _customerService = customerService;
                    _eventPublisher = eventPublisher;
                    _exportManager = exportManager;
                    _externalAuthenticationService = externalAuthenticationService;
                    _gdprService = gdprService;
                    _genericAttributeService = genericAttributeService;
                    _giftCardService = giftCardService; 
                    _localizationService = localizationService;
                    _logger = logger;
                    _newsLetterSubscriptionService = newsLetterSubscriptionService;
                    _orderService = orderService;
                    _pictureService = pictureService;
                    _priceFormatter = priceFormatter;
                    _shoppingCartService = shoppingCartService;
                    _stateProvinceService = stateProvinceService;
                    _storeContext = storeContext;
                    _taxService = taxService;
                    _webHelper = webHelper;
                    _workContext = workContext;
                    _workflowMessageService = workflowMessageService;
                    _localizationSettings = localizationSettings;
                    _mediaSettings = mediaSettings;
                    _storeInformationSettings = storeInformationSettings;
                    _taxSettings = taxSettings;

        }


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        //[ValidateCaptcha]
        [ValidateHoneypot]
        [CheckAccessPublicStore(true)]
        public IActionResult Register(RegisterModel model)
        {

            //check whether registration is allowed
            if (_customerSettings.UserRegistrationType == UserRegistrationType.Disabled)
            {
                var failmodel = _customerModelFactory.PrepareRegisterResultModel((int)UserRegistrationType.Disabled);

                return Json(new
                {
                    code = -1,
                    msg = failmodel.Result,
                    data = new {

                    }
                });
            }
               

            //return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Disabled });
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            return View();
        }


        public IActionResult CheckPhone(string phone)
        {
            return Json(new
            {
                code = "",
                msg ="",
                data=new { }
            });
        }

        /// <summary>
        /// 短信验证手机号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public IActionResult ValidatePhone(string phone)
        {
            return Json(new
            {

            });
        }

    }
}