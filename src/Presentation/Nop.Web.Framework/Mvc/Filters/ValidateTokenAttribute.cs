using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Security;
using Nop.Core.LoginInfo;
using Nop.Services.Logging;
using Nop.Web.Framework.Security.Captcha;
using System.Linq;
using Nop.Services.Customers;

namespace Nop.Web.Framework.Mvc.Filters
{
    /// <summary>
    /// Represents a filter attribute enabling CAPTCHA validation
    /// </summary>
    public class ValidateTokenAttribute : TypeFilterAttribute
    {
        #region Ctor

        /// <summary>
        /// Create instance of the filter attribute 
        /// </summary>
        /// <param name="actionParameterName">The name of the action parameter to which the result will be passed</param>
        public ValidateTokenAttribute(string actionParameterName = "tokenvalid", string actionParameterName1 = "qs_clientidresult") : base(typeof(ValidateTokenAttribute))
        {
            Arguments = new object[] { actionParameterName, actionParameterName1 };
        }

        #endregion

        #region Nested filter

        /// <summary>
        /// Represents a filter enabling CAPTCHA validation
        /// </summary>
        private class ValidateTokenFilter : IActionFilter
        {
            #region Constants

            //private const string CHALLENGE_FIELD_KEY = "recaptcha_challenge_field";
            //private const string RESPONSE_FIELD_KEY = "recaptcha_response_field";
            //private const string G_RESPONSE_FIELD_KEY = "g-recaptcha-response";
            private const string API_TOKEN = "token";
            private const string API_CLIENTID = "qs_clientid";

            #endregion

            #region Fields

            private readonly string _actionParameterName;
            private readonly CaptchaHttpClient _captchaHttpClient;
            private readonly CaptchaSettings _captchaSettings;
            private readonly ILogger _logger;
            private readonly IWorkContext _workContext;
            private readonly ICustomerService _customerService;

            #endregion

            #region Ctor

            public ValidateTokenFilter(string actionParameterName,
                CaptchaHttpClient captchaHttpClient,
                CaptchaSettings captchaSettings,
                ILogger logger,
                ICustomerService customerService,
                IWorkContext workContext)
            {
                _actionParameterName = actionParameterName;
                _captchaHttpClient = captchaHttpClient;
                _captchaSettings = captchaSettings;
                _customerService = customerService;
                _logger = logger;
                _workContext = workContext;
            }

            #endregion

            #region Utilities

            /// <summary>
            /// Validate CAPTCHA
            /// </summary>
            /// <param name="context">A context for action filters</param>
            /// <returns>返回0  表示token 正确 ，1表示要强退  2、表示token不合法</returns>
            protected int ValidateToken(ActionExecutingContext context)
            {
             //   var isValid = false;

                //get form values
                var tokenValue = context.HttpContext.Request.Form[API_TOKEN];
                var clientid = context.HttpContext.Request.Form[API_CLIENTID];

                if (!StringValues.IsNullOrEmpty(tokenValue) || !StringValues.IsNullOrEmpty(clientid))
                {
                    //validate request
                    try
                    {
                        Customer customer = null;
                        var apitetoken = new AccountToken();
                        apitetoken = AccountToken.Deserialize(tokenValue);
                        //  var islogin = false;///校验token是否正确
                        if (apitetoken != null && clientid == apitetoken.ClientId)
                        {

                            int cid = 0;
                            Int32.TryParse(apitetoken.ID, out cid);

                            customer = _customerService.GetCustomerById(cid);
                            _workContext.CurrentCustomer = customer;

                            /// 1.0如果之前登录过 并保存过token
                            if (!string.IsNullOrEmpty(customer.LastToken))
                            {
                                var lasttoken = AccountToken.Deserialize(customer.LastToken);
                                if (lasttoken != null)
                                {
                                    if (apitetoken.ClientId == lasttoken.ClientId && apitetoken.UserName == lasttoken.UserName)
                                    {
                                        return 0;
                                    }
                                    else
                                    {
                                        return 1;
                                    }
                                }
                                else
                                {
                                    return 0;
                                }
                            }
                            else
                            {
                                return 0;
                            }
                            //if ()
                            //{
                            //    _workContext.CurrentCustomer.LastIpAddress = currentIpAddress;

                            //}
                        }
                        else
                        {
                            return 2;
                        }
                    }
                    catch (Exception exception)
                    {
                        _logger.Error("Error occurred on Token validation", exception, _workContext.CurrentCustomer);
                        return 2;
                    }
                }
                else
                {

                    return 2;
                    //isValid = false;
                   // return false;
                }

               // return isValid;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Called before the action executes, after model binding is complete
            /// </summary>
            /// <param name="context">A context for action filters</param>
            public void OnActionExecuting(ActionExecutingContext context)
            {
                if (context == null)
                    throw new ArgumentNullException(nameof(context));

                if (!DataSettingsManager.DatabaseIsInstalled)
                    return;
                int result = -1;
                //whether CAPTCHA is enabled
                if ( context.HttpContext?.Request != null)
                {
                    result = ValidateToken(context);
                    //push the validation result as an action parameter
                    context.ActionArguments[_actionParameterName] = result;
                }
                else
                    context.ActionArguments[_actionParameterName] = 2 ;

                
                if (result == 1)
                {
                    context.Result = new JsonResult(new
                    {

                        code = -2,
                        msg = "账号已在其他设备登录",
                        data = false
                    });
                }
                if (result == 2)
                {
                    context.Result = new JsonResult(new
                    {

                        code = -1,
                        msg = "无效凭证",
                        data = false
                    });
                }


            }

            /// <summary>
            /// Called after the action executes, before the action result
            /// </summary>
            /// <param name="context">A context for action filters</param>
            public void OnActionExecuted(ActionExecutedContext context)
            {
                //do nothing
            }

            #endregion
        }

        #endregion
    }
}