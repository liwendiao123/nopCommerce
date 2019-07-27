using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain;
using Nop.Core.Domain.Catalog;
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
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Models.Customer;
using Microsoft.Extensions.Primitives;
using Nop.Core.Domain.Messages;
using Nop.Services.Authentication;
using Nop.Web.Models.Api;
using Nop.Services.Security;
using Nop.Web.Infrastructure;
using System.Diagnostics;
using Nop.Web.Models.AliSms;
using Nop.Web.Models.Api.Ali;
using Nop.Core.Domain.Messages.SMS;
using Nop.Web.Areas.Admin.Models.TableOfContent;
using Nop.Web.Models.AiBook;
using Nop.Services.TableOfContent;
using Nop.Core.Infrastructure.Ex;

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
        private readonly ISmsService _smsService;
        private readonly IBookDirService _bookDirService;
        private readonly IDepartmentService _departmentService;
       /// private readonly

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
        private readonly IProductService _productService;
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
                    IDepartmentService departmentService,
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
                    IProductService productService,
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
                    ISmsService smsService,
                    IBookDirService bookDirService,
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
                    _productService = productService;
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
                    _smsService = smsService;
                    _taxSettings = taxSettings;
                    _bookDirService = bookDirService;
                    _departmentService = departmentService;
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
        public IActionResult Register(ApiRegisterModel model, string returnUrl, bool captchaValid, IFormCollection form)
        {

            if (string.IsNullOrEmpty(model.Email))
            {
                model.Email = Guid.NewGuid().ToString("N") + "@163.com";
            }

            model.Email = model.Email.Trim();

            SmsMsgRecord record = new SmsMsgRecord();
            if (model != null)
            {
                model.LastName = model.Name;
            }

            if (string.IsNullOrEmpty(model.SmsCode))
            {
                return Json(new
                {
                    code = -1,
                    msg = "注册失败:手机验证码不能为空",
                    data = false
                });
            }

            else
            {
                 record = new SmsMsgRecord {

                     Phone = model.Phone,
                     Type = 0,
                     AppId  = AliSmsManager.accessKeyId,
                     TemplateCode = model.SmsCode
                };
               var result = _smsService.CheckMsgValid(record);

                if (!result.Result)
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "注册失败:手机验证码不能为空",
                        data = false
                    });
                }
            }

            var department = _departmentService.GetDepById(model.DepartmentId);

            string depName = string.Empty;

            if (department == null)
            {

                if (!string.IsNullOrEmpty(model.SchoolName))
                {
                    var dep = new Department()
                    {
                        Name = model.SchoolName,
                        CreatedOnUtc = DateTime.Now,
                        Desc = "用户输入",
                        Active = true,
                        ContactPerson = model.Name,
                        Tel = model.Phone,
                        VatCode = Guid.NewGuid().ToString("N"),
                        UpdatedOnUtc = DateTime.Now


                    };
                    var deps = _departmentService.GetAllDeps(true);
                    var cschool = deps.FirstOrDefault(x => x.Name.Equals(model.SchoolName));
                    bool result = false;
                    if (cschool == null)
                    {
                         result = _departmentService.InsertDep(dep);
                    }
                    else
                    {
                        result = true;
                    }
                   


                    if (result)
                    {
                      
                        if (cschool == null)
                        {
                            return Json(new
                            {
                                code = 0,
                                msg = "注册失败:请指定一个学校",
                                data = false
                            });
                        }
                        else
                        {
                            model.DepartmentId = cschool.Id;
                        }
                    }
                }
                else
                {
                    return Json(new
                    {
                        code = 0,
                        msg = "注册失败:请指定一个学校",
                        data = false
                    });
                }
      
            }
            else
            {
                depName = department.Name;
                model.DepartmentId = department.Id;
            }

            //check whether registration is allowed
            if (_customerSettings.UserRegistrationType == UserRegistrationType.Disabled)
                return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Disabled });
            if (_workContext.CurrentCustomer.IsRegistered())
            {
                //Already registered customer. 
                _authenticationService.SignOut();

                //raise logged out event       
                _eventPublisher.Publish(new CustomerLoggedOutEvent(_workContext.CurrentCustomer));

                //Save a new record
                _workContext.CurrentCustomer = _customerService.InsertGuestCustomer();
            }
            var customer = _workContext.CurrentCustomer;
            customer.RegisteredInStoreId = _storeContext.CurrentStore.Id;
            //custom customer attributes
            var customerAttributesXml = ParseCustomCustomerAttributes(form);
            var customerAttributeWarnings = _customerAttributeParser.GetAttributeWarnings(customerAttributesXml);
            foreach (var error in customerAttributeWarnings)
            {
                ModelState.AddModelError("", error);
            }
            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnRegistrationPage && !captchaValid)
            {
                ModelState.AddModelError("", _localizationService.GetResource("Common.WrongCaptchaMessage"));
            }
            if (ModelState.IsValid)
            {
                if (_customerSettings.UsernamesEnabled && model.UserName != null)
                {
                    model.UserName = model.UserName.Trim();
                }
                else if (_customerSettings.UsernamesEnabled)
                {
                    if (_customerSettings.PhoneEnabled)
                    {
                        model.UserName = model.Phone;
                    }
                    else
                    {
                        model.UserName = model.Email;
                    }
                }
                customer.SystemName = "ApiRegister";
                var isApproved = _customerSettings.UserRegistrationType == UserRegistrationType.Standard;
                var registrationRequest = new CustomerRegistrationRequest(customer,
                    model.Email,                    
                    _customerSettings.UsernamesEnabled ? model.UserName : model.Email,
                    model.Password,
                    _customerSettings.DefaultPasswordFormat,
                    _storeContext.CurrentStore.Id,
                    isApproved);

                registrationRequest.Customer.DepartmentId = model.DepartmentId;
                if (model.Occupation == 0)
                {
                    registrationRequest.RoleName = "Student";
                }
                else if (model.Occupation == 1)
                {
                    registrationRequest.RoleName = "Teacher";
                }
                else if (model.Occupation == 2)
                {
                    registrationRequest.RoleName = "guarden";
                }
                else
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "注册失败：请明确用户是老师或学生",
                        data = false
                    });
                }

                var registrationResult = _customerRegistrationService.RegisterCustomer(registrationRequest);
                if (registrationResult.Success)
                {
                    //properties
                    if (_dateTimeSettings.AllowCustomersToSetTimeZone)
                    {
                        _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.TimeZoneIdAttribute, model.TimeZoneId);
                    }

                    //form fields
                    if (_customerSettings.GenderEnabled)
                        _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.GenderAttribute, model.Gender);
                    // _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.FirstNameAttribute, model.FirstName);
                    _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.LastNameAttribute, model.Name);

                    if (_customerSettings.CompanyEnabled)
                        _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.CompanyAttribute, model.Company);
                    ///保存电话号码
                    if (_customerSettings.PhoneEnabled)
                        _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.PhoneAttribute, model.Phone);
                    ///保存 邀请码
                    if (_customerSettings.InviteCodeEnabled)
                    {
                        _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.InviteCodeAttribute, model.InviteCode);
                    }
                    if (_customerSettings.IdcardImgEnabled)
                    {
                        _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.IdCardImgAttribute, model.ImgUrl);
                    }
                    ///签订保密协议
                    if (_customerSettings.AcceptPrivacyPolicyEnabled)
                    {
                        //privacy policy is required
                        //GDPR
                        if (_gdprSettings.GdprEnabled && _gdprSettings.LogPrivacyPolicyConsent)
                        {
                            _gdprService.InsertLog(customer, 0, GdprRequestType.ConsentAgree, _localizationService.GetResource("Gdpr.Consent.PrivacyPolicy"));
                        }
                    }
                    //save customer attributes
                    _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.CustomCustomerAttributes, customerAttributesXml);


                    //notifications
                    if (_customerSettings.NotifyNewCustomerRegistration)
                        _workflowMessageService.SendCustomerRegisteredNotificationMessage(customer,
                            _localizationSettings.DefaultAdminLanguageId);

                    //raise event       
                    _eventPublisher.Publish(new CustomerRegisteredEvent(customer));

                    //switch (_customerSettings.UserRegistrationType)
                    //{
                    //    case UserRegistrationType.EmailValidation:
                    //        {
                    //            //email validation message
                    //            _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.AccountActivationTokenAttribute, Guid.NewGuid().ToString());
                    //            _workflowMessageService.SendCustomerEmailValidationMessage(customer, _workContext.WorkingLanguage.Id);

                    //            //result
                    //            return RedirectToRoute("RegisterResult",
                    //                new { resultId = (int)UserRegistrationType.EmailValidation });
                    //        }
                    //    case UserRegistrationType.AdminApproval:
                    //        {
                    //            return RedirectToRoute("RegisterResult",
                    //                new { resultId = (int)UserRegistrationType.AdminApproval });
                    //        }
                    //    case UserRegistrationType.Standard:
                    //        {
                    //            //send customer welcome message
                    //            _workflowMessageService.SendCustomerWelcomeMessage(customer, _workContext.WorkingLanguage.Id);

                    //            var redirectUrl = Url.RouteUrl("RegisterResult",
                    //                new { resultId = (int)UserRegistrationType.Standard, returnUrl }, _webHelper.CurrentRequestProtocol);
                    //            return Redirect(redirectUrl);
                    //        }
                    //    default:
                    //        {
                    //            return RedirectToRoute("Homepage");
                    //        }
                    //}
                }

                //errors
                foreach (var error in registrationResult.Errors)
                    ModelState.AddModelError("", error);


                if (registrationResult.Errors.Count > 0)
                {
                    return Json(new
                    {
                        code = -1,
                        msg = string.Join(",", registrationResult.Errors),
                        data = false

                    });
                }
            }
            else
            {
                return Json(new
                {
                    code = -1,
                    msg = "未按照要求填写注册信息",
                    data= false

                });
            }

            _smsService.ApplySms(record);

            //If we got this far, something failed, redisplay form
          //  model = _customerModelFactory.PrepareRegisterModel(model, true, customerAttributesXml);
        
            return Json(               
                new {
                code = 0,
                msg = "信息获取成功",

                data=new{
                    Id = model.UserName,
                    Phone = model.Phone,
                    Name = model.LastName ?? "",
                    Email = model.Email ?? "",
                    Occupation = model.Occupation == 7 ? "学生" : "老师",
                    SchoolName = depName,
                    DepId = model.DepartmentId
                }
              
            });

            //return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Disabled });
          //  return View();
        }



        #region
        protected virtual string ParseCustomCustomerAttributes(IFormCollection form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            var attributesXml = "";
            var attributes = _customerAttributeService.GetAllCustomerAttributes();
            foreach (var attribute in attributes)
            {
                var controlId = $"{NopAttributePrefixDefaults.Customer}{attribute.Id}";
                switch (attribute.AttributeControlType)
                {
                    case AttributeControlType.DropdownList:
                    case AttributeControlType.RadioList:
                        {
                            var ctrlAttributes = form[controlId];
                            if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                            {
                                var selectedAttributeId = int.Parse(ctrlAttributes);
                                if (selectedAttributeId > 0)
                                    attributesXml = _customerAttributeParser.AddCustomerAttribute(attributesXml,
                                        attribute, selectedAttributeId.ToString());
                            }
                        }
                        break;
                    case AttributeControlType.Checkboxes:
                        {
                            var cblAttributes = form[controlId];
                            if (!StringValues.IsNullOrEmpty(cblAttributes))
                            {
                                foreach (var item in cblAttributes.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                                {
                                    var selectedAttributeId = int.Parse(item);
                                    if (selectedAttributeId > 0)
                                        attributesXml = _customerAttributeParser.AddCustomerAttribute(attributesXml,
                                            attribute, selectedAttributeId.ToString());
                                }
                            }
                        }
                        break;
                    case AttributeControlType.ReadonlyCheckboxes:
                        {
                            //load read-only (already server-side selected) values
                            var attributeValues = _customerAttributeService.GetCustomerAttributeValues(attribute.Id);
                            foreach (var selectedAttributeId in attributeValues
                                .Where(v => v.IsPreSelected)
                                .Select(v => v.Id)
                                .ToList())
                            {
                                attributesXml = _customerAttributeParser.AddCustomerAttribute(attributesXml,
                                    attribute, selectedAttributeId.ToString());
                            }
                        }
                        break;
                    case AttributeControlType.TextBox:
                    case AttributeControlType.MultilineTextbox:
                        {
                            var ctrlAttributes = form[controlId];
                            if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                            {
                                var enteredText = ctrlAttributes.ToString().Trim();
                                attributesXml = _customerAttributeParser.AddCustomerAttribute(attributesXml,
                                    attribute, enteredText);
                            }
                        }
                        break;
                    case AttributeControlType.Datepicker:
                    case AttributeControlType.ColorSquares:
                    case AttributeControlType.ImageSquares:
                    case AttributeControlType.FileUpload:
                    //not supported customer attributes
                    default:
                        break;
                }
            }

            return attributesXml;
        }
        #endregion
        public IActionResult Login()
        {
           return View();
        }
        [HttpPost]
        public IActionResult Login(string userName,string password)
        {
            //validate CAPTCHA
            //if (_captchaSettings.Enabled && _captchaSettings.ShowOnLoginPage && !captchaValid)
            //{
            //    ModelState.AddModelError("", _localizationService.GetResource("Common.WrongCaptchaMessage"));
            //}


            string name = "";
            string inviteCode = string.Empty;
            string imgurl = string.Empty;

            LoginModel model = new LoginModel()
            {
                 Email = "li@163.com",
                 Username = userName,
                 Password = password
            };

            Customer _customer = null;

            if (ModelState.IsValid)
            {
                if (_customerSettings.UsernamesEnabled && model.Username != null)
                {
                    model.Username = model.Username.Trim();
                }
                var loginResult = _customerRegistrationService.ValidateCustomer(_customerSettings.UsernamesEnabled ? model.Username : model.Email, model.Password);
                switch (loginResult)
                {
                    case CustomerLoginResults.Successful:
                        {
                            var customer = _customerSettings.UsernamesEnabled
                                ? _customerService.GetCustomerByUsername(model.Username)
                                : _customerService.GetCustomerByEmail(model.Email);

                            //migrate shopping cart
                            //_shoppingCartService.MigrateShoppingCart(_workContext.CurrentCustomer, customer, true);
                            _customer = customer;
                            //sign in new customer
                            _authenticationService.SignIn(customer, model.RememberMe);

                            //raise event       
                            _eventPublisher.Publish(new CustomerLoggedinEvent(customer));

                            //activity log
                            _customerActivityService.InsertActivity(customer, "PublicStore.Login",
                                _localizationService.GetResource("ActivityLog.PublicStore.Login"), customer);

                            imgurl = _genericAttributeService.GetAttribute<string>(_customer, NopCustomerDefaults.IdCardImgAttribute);
                            name = _genericAttributeService.GetAttribute<string>(_customer, NopCustomerDefaults.LastNameAttribute);
                            var dep = _departmentService.GetDepById(_customer.DepartmentId);
                            inviteCode = _genericAttributeService.GetAttribute<string>(_customer, NopCustomerDefaults.InviteCodeAttribute);
                            return Json(new {
                                code = 0,
                                msg="登录成功",
                                data = new {
                                    Id = _customer.Id,
                                    UserName = _customer.Username,
                                    Name = name,
                                    Phone = _customer.Username,
                                    Email = _customer.Email,
                                    Token = "",
                                    InviteCode = inviteCode,
                                    CardImgUrl = imgurl,
                                    SchoolName =dep==null ?"七三科技":dep.Name,
                                    DepartmentId = _customer.DepartmentId,
                                    Role =string.Join(",", _customer.CustomerCustomerRoleMappings.Select(x=>x.CustomerRole.Name).ToList())
                                }


                            });
                            //if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                            //    return RedirectToRoute("Homepage");

                            //return Redirect(returnUrl);
                        }

                        //break;
                    case CustomerLoginResults.CustomerNotExist:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.CustomerNotExist"));
                        break;
                    case CustomerLoginResults.Deleted:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.Deleted"));
                        break;
                    case CustomerLoginResults.NotActive:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.NotActive"));
                        break;
                    case CustomerLoginResults.NotRegistered:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.NotRegistered"));
                        break;
                    case CustomerLoginResults.LockedOut:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.LockedOut"));
                        break;
                    case CustomerLoginResults.WrongPassword:
                    default:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials"));
                        break;
                }
            }

            //If we got this far, something failed, redisplay form
            model = _customerModelFactory.PrepareLoginModel(model.CheckoutAsGuest);
            return Json(new {
                code = -1,
                 msg ="登录失败;原因："+string.Join(",", ModelState.Root.Errors.Select(x=>x.ErrorMessage))              
            });
          //  return View(model);
        }
        public IActionResult CheckPhone(string phone)
        {


            try
            {

                if (string.IsNullOrEmpty(phone))
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "手机号码格式错误",
                        data = false
                    });
                }


                if (!phone.CheckMobile())
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "手机号码格式错误",
                        data = false
                    });
                }

                var result = _customerService.GetCustomerByUsername(phone);

                if (result != null && !result.Deleted)
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "该手机号已被注册",
                        data = false
                    });
                }

                return Json(new
                {
                    code = 0,
                    msg = "恭喜您！号码可用",
                    data = true
                });

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = -1,
                    msg = ex.Message,
                    data = false
                });
            }



          
        }
        /// <summary>
        /// 短信验证手机号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public async Task<IActionResult> ValidatePhoneAsync(RequestValidCode request)
        {
            if (request == null || string.IsNullOrEmpty( request.Phone))
            {
                return Json(new
                {
                    code = -1,
                    msg = "请求失败,手机号不能为空",
                    data = false
                });
            }


            string title = "注册账户验证";

            var smr = new SmsMsgRecord()
            {
                AppId = "LTAIgSHNbd8oGL92",
                Phone = request.Phone,
                Content = "",
                TemplateCode = "",
                IsRead = 0,
                CreateTime = DateTime.Now,
                SysName = "Ali",
                Title = title,
               
            };

            var sms = new SmsObject
            {
                Mobile = request.Phone ?? "18588276558",
                Signature = "七三科技",
                TempletKey = "SMS_148990154",
               // Data = data,
                OutId = "qskjtoken"
            };

            IDictionary<string, string> data = new Dictionary<string, string>();
            var randomcode = DateTime.Now.ToString("ffffff");
            data.Add("code", randomcode);

            AliSmsTemp resultem = new AliSmsTemp();

            switch (request.Type)
            {
                case 0:
                    title = "手机注册验证";
                    resultem = AliSmsTemplateManager.GetAliSmsTemByType(0);  
                   
                    break;
                case 1:
                    title = "重置密码";
                    resultem = AliSmsTemplateManager.GetAliSmsTemByType(1);           
                    break;
                case 2:
                    title = "登录";
                    resultem = AliSmsTemplateManager.GetAliSmsTemByType(2);
                    break;
                case 3:
                    title = "更新角色";
                    break;
            }
            smr.Title = title;
            smr.Content = resultem == null ? "" :string.Format( resultem.MsgTemContent,randomcode);
            smr.Type = resultem == null ? 0 : resultem.Type;
            sms.TempletKey = resultem == null ? sms.TempletKey : resultem.MsgCode;
            var result =  _smsService.CheckMsgValid(smr);
            if (result.Code == Core.Infrastructure.ErrorCode.mobile_sms_frequently)
            {
                return Json(new
                {
                    code = 0,
                    msg = result.Msg,
                    data = smr.TemplateCode
                });
            }
            sms.Data = data;
            var res =await new AliyunSmsSender().Send(sms);
            if (res.success)
            {
                smr.TemplateCode = randomcode;
               _smsService.SendMsg(smr);
            }

            if (!res.success)
            {
                return Json(new
                {
                    code = -1,
                    msg  = "发送验证码失败：原因："+ res.response
                });
            }

            return Json(new
            {
                code = 0,
                msg = "验证码已发送",
                data = randomcode
            });
        }
       /// <summary>
    /// 获取用户基本信息
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
        public IActionResult GetUserInfo(string userName)
        {
          //var result =  _customerService.GetCustomerByUsername(userName);

          // var  phone = _genericAttributeService.GetAttribute<string>(result, NopCustomerDefaults.PhoneAttribute);
            return Json(new
            {

                code = 0,
                msg = "获取成功",
                data =new {
                    Id = 1,
                    Phone = "185******58",
                    DepName = "南宁三中",
                   
                    RoleName ="学生",
                    Email ="2811545@qq.com",
                    InviteCode = "00001",
                    IdCard = "",
                    IdCardImg = "http://arbookresouce.73data.cn/book/img/sy_img_02.png",
                },
               
            });
        }
        /// <summary>
        /// 获取学习进度
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// 
        public IActionResult LearnProgress(string userName)
        {
          var result =   _productService.SearchProducts();

            return Json(new
            {
                code = 0,
                msg = "获取成功",
                data = result.ToList().Select(x=>new {
                    Id = x.Id,
                    Name = x.Name,
                    Percent = 0.2,
                    DisplayOrder =   x.DisplayOrder,
                    LastNode = new {
                        Id = 1,  //章节ID
                        PId = "", //上级ID
                        IsLock = true,///是否已经购买 解锁      
                        Name = "地壳的圈层结构", //章节名称
                        IsRead = false,
                        ComplexLevel = "", //收费费复杂知识点
                        ImgUrl = "http://arbookresouce.73data.cn/book/img/sy_img_02.png",//封面展示
                        BookNodeUrl = "" //获取对应知识点 Url 
                    }
                })
            });

        }
        public IActionResult GetRole()
        {
           var list =  _customerService.GetAllCustomerRoles().ToList();


            return Json(new
            {
                code = 0,
                msg = "获取成功",
                data = list.Where(x => !x.IsSystemRole).Select(x=> new {
                    Id = x.Id,
                    Name = x.Name,
                    SystemName = x.SystemName,
                    IsTeacher = x.Id == 6
                }).ToList()

            });
        }
        [HttpPost]
        public IActionResult UpdateInfo(UpdateCustomerInfoModel ucim)
        {

            ///1.0获取用户基本信息
            var customer = _customerService.GetCustomerById(ucim.Id);

            if (customer == null || customer.Deleted)
            {
                return Json(new
                {
                    code = -1,
                    msg = "参数格式错误或者该用户已被禁用",
                    data = false
                });
            }

           /// 2.0获取用户所有角色
            var allCustomerRoles = _customerService.GetAllCustomerRoles(true);

            var curroleId = allCustomerRoles.FirstOrDefault(x => x.Id == ucim.RoleId);

            var newCustomerRoles = new List<CustomerRole>();
            foreach (var customerRole in allCustomerRoles)
            {
                if (ucim.RoleId == (customerRole.Id))
                    newCustomerRoles.Add(customerRole);
            }
            
            var customerRolesError = ValidateCustomerRoles(newCustomerRoles, customer.CustomerRoles);
            if (!string.IsNullOrEmpty(customerRolesError))
            {
                ModelState.AddModelError(string.Empty, customerRolesError);

                return Json(new
                {
                    code = -1,
                    msg = customerRolesError,
                    data = false
                });
                //_notificationService.ErrorNotification(customerRolesError);
            }
            // Ensure that valid email address is entered if Registered role is checked to avoid registered customers with empty email address
            if (newCustomerRoles.Any() && newCustomerRoles.FirstOrDefault(c => c.SystemName == NopCustomerDefaults.RegisteredRoleName) != null &&
                !CommonHelper.IsValidEmail(ucim.Email))
            {
                ModelState.AddModelError(string.Empty, _localizationService.GetResource("Admin.Customers.Customers.ValidEmailRequiredRegisteredRole"));

                return Json(new
                {
                    code = -1,
                    msg = _localizationService.GetResource("Admin.Customers.Customers.ValidEmailRequiredRegisteredRole"),
                    data = false
                });

                //  _notificationService.ErrorNotification(_localizationService.GetResource("Admin.Customers.Customers.ValidEmailRequiredRegisteredRole"));
            }
            //username
            if (_customerSettings.UsernamesEnabled)
            {
                if (!string.IsNullOrWhiteSpace(ucim.Username))
                    _customerRegistrationService.SetUsername(customer, ucim.Username);
                else
                    customer.Username = ucim.Username;
            }     
            //email
            if (!string.IsNullOrWhiteSpace(ucim.Email))
                _customerRegistrationService.SetEmail(customer, ucim.Email, false);
            else
                customer.Email = ucim.Email;
            customer.DepartmentId = ucim.DepId;
           // customer.InivteCode = ucim.InviteCode;

            if (_customerSettings.PhoneEnabled)
                _genericAttributeService.SaveAttribute(customer, NopCustomerDefaults.PhoneAttribute, ucim.Phone);


            //customer roles
            foreach (var customerRole in allCustomerRoles)
            {
                //ensure that the current customer cannot add/remove to/from "Administrators" system role
                //if he's not an admin himself
                if (customerRole.SystemName == NopCustomerDefaults.AdministratorsRoleName &&
                    !_workContext.CurrentCustomer.IsAdmin())
                    continue;

                if (ucim.RoleId==(customerRole.Id))
                {
                    //new role
                    if (customer.CustomerCustomerRoleMappings.Count(mapping => mapping.CustomerRoleId == customerRole.Id) == 0)
                        customer.AddCustomerRoleMapping(new CustomerCustomerRoleMapping { CustomerRole = customerRole });
                }
                else
                {
                    //prevent attempts to delete the administrator role from the user, if the user is the last active administrator
                    if (customerRole.SystemName == NopCustomerDefaults.AdministratorsRoleName && !SecondAdminAccountExists(customer))
                    {
                       // _notificationService.ErrorNotification(_localizationService.GetResource("Admin.Customers.Customers.AdminAccountShouldExists.DeleteRole"));
                        continue;
                    }

                    //remove role
                    if (customer.CustomerCustomerRoleMappings.Count(mapping => mapping.CustomerRoleId == customerRole.Id) > 0)
                    {
                        customer.RemoveCustomerRoleMapping(
                            customer.CustomerCustomerRoleMappings.FirstOrDefault(mapping => mapping.CustomerRoleId == customerRole.Id));

                    }
                }
            }

            _customerService.UpdateCustomer(customer);
            _customerActivityService.InsertActivity("EditCustomerSelf",
                       string.Format(_localizationService.GetResource("ActivityLog.EditCustomer"), customer.Id), customer);
            return Json(new
            {
                code = 0,
                msg = "修改成功",
                data = true
            });
        }
        /// <summary>
        /// 校验用户角色
        /// </summary>
        /// <param name="customerRoles"></param>
        /// <param name="existingCustomerRoles"></param>
        /// <returns></returns>
        protected virtual string ValidateCustomerRoles(IList<CustomerRole> customerRoles, IList<CustomerRole> existingCustomerRoles)
        {
            if (customerRoles == null)
                throw new ArgumentNullException(nameof(customerRoles));

            if (existingCustomerRoles == null)
                throw new ArgumentNullException(nameof(existingCustomerRoles));

            //check ACL permission to manage customer roles
            var rolesToAdd = customerRoles.Except(existingCustomerRoles);
            var rolesToDelete = existingCustomerRoles.Except(customerRoles);
            if (rolesToAdd.Where(role => role.SystemName != NopCustomerDefaults.RegisteredRoleName).Any() || rolesToDelete.Any())
            {
                //if (!_permissionService.Authorize(StandardPermissionProvider.ManageAcl))
                //    return _localizationService.GetResource("Admin.Customers.Customers.CustomerRolesManagingError");
            }

            //ensure a customer is not added to both 'Guests' and 'Registered' customer roles
            //ensure that a customer is in at least one required role ('Guests' and 'Registered')
            var isInGuestsRole = customerRoles.FirstOrDefault(cr => cr.SystemName == NopCustomerDefaults.GuestsRoleName) != null;
            var isInRegisteredRole = customerRoles.FirstOrDefault(cr => cr.SystemName == NopCustomerDefaults.RegisteredRoleName) != null;
            if (isInGuestsRole && isInRegisteredRole)
                return _localizationService.GetResource("Admin.Customers.Customers.GuestsAndRegisteredRolesError");
            if (!isInGuestsRole && !isInRegisteredRole)
                return _localizationService.GetResource("Admin.Customers.Customers.AddCustomerToGuestsOrRegisteredRoleError");

            //no errors
            return string.Empty;
        }
        private bool SecondAdminAccountExists(Customer customer)
        {
            var customers = _customerService.GetAllCustomers(customerRoleIds: new[] { _customerService.GetCustomerRoleBySystemName(NopCustomerDefaults.AdministratorsRoleName).Id });

            return customers.Any(c => c.Active && c.Id != customer.Id);
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel model,string userName)
        {
            //if (!_workContext.CurrentCustomer.IsRegistered())
            //    return Challenge();

            var customer = _customerService.GetCustomerByUsername(userName);

            if (ModelState.IsValid)
            {
                var changePasswordRequest = new ChangePasswordRequest(customer.Email,userName,
                    true, _customerSettings.DefaultPasswordFormat, model.NewPassword, model.OldPassword);
                var changePasswordResult = _customerRegistrationService.ChangePassword(changePasswordRequest);
                if (changePasswordResult.Success)
                {
                    model.Result = _localizationService.GetResource("Account.ChangePassword.Success");
                    return Json(new {
                        code = 0,
                        msg ="密码修改成功", //model.Result,
                        data = true
                    });
                }

                //errors
                foreach (var error in changePasswordResult.Errors)
                    ModelState.AddModelError("", error);
            }

            //If we got this far, something failed, redisplay form
            return Json(new {
                code = -1,
                msg = model.Result??"修改失败",
                data = false

            });
        }
        // public IActionResult Get
        public IActionResult GetMyCollection(string userName)
        {
            BookDirSearchModel searchModel = new BookDirSearchModel
            {
                BookID = 0,
                BookDirId = 0
            };

            
            var result = _bookDirService.GetAllBookDirsData("", 0, 0, 0).ToList();
            result.ForEach(x =>
            {
                x.BookNodeUrl = Request.Scheme + "://" + Request.Host + "BookNode/GetData?id=" + x.Id;
            });
            //  var model = _bookDirFactory.PrepareBookDirSearchModel(searchModel, new BookDirModel());
            var treeresult = result.ToList();
            var list = new List<BookDirTreeModel>();
            treeresult.ToList().ForEach(x =>
            {
                list.Add(new BookDirTreeModel
                {
                    Id = x.Id,  //章节ID
                    PId = x.ParentBookDirId, //上级ID
                    IsLock = true,///是否已经购买 解锁
                    BookID = x.BookID, //所属课本ID
                    Name = x.Name, //章节名称
                    IsRead = false,
                    Description = x.Description,//章节描述
                    PriceRanges = x.PriceRanges ?? "0",//价格描述  如果为零 则免费 否则展示需要付费的价格
                    DisplayOrder = x.DisplayOrder,//展示顺序
                    IsLastNode = x.IsLastNode,  //是否为知识点
                    ComplexLevel = x.ComplexLevel, //收费费复杂知识点
                    ImgUrl = "http://arbookresouce.73data.cn/book/img/sy_img_02.png",//封面展示
                    //获取对应知识点 Url"
                    BookNodeUrl = "http://www.73data.cn/EduProject/Sports.php?id=" + x.Id

                });

            });
            var resl = new List<BookDirTreeModel>();
            var resl1 = SortBookDirsForTree(list, resl, new List<int>(), 0);
            resl = resl1.ToList();
            var bookid =  treeresult.Select(x => x.BookID).Distinct().ToList();


            var books = _productService.SearchProducts();

           var   booklist = books.Where(x => x.Id == 47 || x.Id == 48 || x.Id == 46).ToList();
            return Json(new
            {
                code = 0,
                msg = "",
                data = booklist.Select(x=> new{
                    Id = x.Id,
                    pecent =0.21,
                    Name = x.Name,
                    BookDir = resl.Where(y=>y.BookID == x.Id).ToList()
                }).ToList()
            });
        }
        /// <summary>
        /// Sort categories for tree representation
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="parentId">Parent category identifier</param>
        /// <param name="ignoreCategoriesWithoutExistingParent">A value indicating whether categories without parent category in provided category list (source) should be ignored</param>
        /// <returns>Sorted categories</returns>
        public virtual IList<BookDirTreeModel> SortBookDirsForTree(IList<BookDirTreeModel> source, List<BookDirTreeModel> list, List<int> ids, int parentId = 0,
            bool ignoreCategoriesWithoutExistingParent = false)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            //var result = new List<BookDirTreeModel>();
            if (list == null)
            {
                list = new List<BookDirTreeModel>();
            }

            if (ids == null)
            {
                ids = new List<int>();
            }
            var result = source.FirstOrDefault(x => x.Id == parentId);
            var children = source.Where(c => c.PId == parentId).ToList();

            if (parentId == 0)
            {
                children = source.Where(c => c.PId < 1).ToList();
            }
            if (result != null)
            {

                if (!ids.Contains(result.Id))
                {
                    list.Add(result);
                    ids.Add(result.Id);
                }

                if (children != null && children.Count > 0)
                {


                    // result.Children.AddRange(children);
                    children.Select(x => x).ToList().ForEach(x =>
                    {

                        if (!ids.Contains(x.Id))
                        {

                            ids.Add(x.Id);
                        }

                        if (!result.Children.Exists(c => c.Id == x.Id))
                        {
                            result.Children.Add(x);
                        }

                    });


                }

            }
            else
            {
                //list.AddRange(children);
                children.Select(x => x).ToList().ForEach(x =>
                {
                    if (!ids.Contains(x.Id))
                    {
                        ids.Add(x.Id);
                    }
                    if (!list.Exists(c => c.Id == x.Id))
                    {
                        list.Add(x);
                    }
                });
            }
            foreach (var cat in children)
            {
                SortBookDirsForTree(source, list, ids, cat.Id, true);
            }
            return list;
        }

        public virtual IActionResult ValidInviteCode(string code)
        {


           ///


            if (code.Equals("001"))
            {

                return Json(new
                {
                    code = 0,
                    msg = "验证成功",
                    data = true
                });
            }
            else
            {
                return Json(new
                {
                    code = -1,
                    msg = "邀请码不存在",
                    data = false
                });
            }
                
        }
    }
}