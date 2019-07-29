using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Payments.WeixinPay.Components
{
    [ViewComponent(Name = "PaymentWeixinPay")]
    public class PaymentWeixinPayViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Plugins/Payments.WeixinPay/Views/PaymentWeixinPay/PaymentInfo.cshtml");
        }
    }
}
