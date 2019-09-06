using System;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Customers;
using Nop.Core.LoginInfo;
using Nop.Services.Customers;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Security;

namespace Nop.Web.Controllers
{
    [HttpsRequirement(SslRequirement.NoMatter)]
    [WwwRequirement]
    [CheckAccessPublicStore]
    [CheckAccessClosedStore]
    [CheckLanguageSeoCode]
    [CheckDiscountCoupon]
    [CheckAffiliate]
    public abstract partial class BasePublicController : BaseController
    {

      ///  public Customer  Current

        protected virtual IActionResult InvokeHttp404()
        {
            Response.StatusCode = 404;
            return new EmptyResult();
        }

        [NonAction]
        protected int ValidateToken(string token,string clientId, ICustomerService _customerService)
        {
            var tokenValue = token;
            var clientid = clientId;

            if (!string.IsNullOrEmpty(tokenValue) || !string.IsNullOrEmpty(clientid))
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

                        if (customer == null)
                        {
                            return 2;
                        }

                        if(customer.Deleted)
                        {
                            return 3;
                        }
                        // _workContext.CurrentCustomer = customer;
                        //customer.LastToken = token;
                        //_customerService.UpdateCustomer(customer);
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
                        return 2 ;
                    }
                }
                catch (Exception exception)
                {
                  // _logger.Error("Error occurred on Token validation", exception, _workContext.CurrentCustomer);
                    return 2;
                }
            }
            else
            {

                return 2;
                //isValid = false;
                // return false;
            }

            //return false;
        }


      
    }
}