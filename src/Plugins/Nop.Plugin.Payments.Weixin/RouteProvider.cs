using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Payments.WeixinPay
{
    public partial class RouteProvider : IRouteProvider
    {
        #region Methods

        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            //Notify
            routeBuilder.MapRoute("Plugin.Payments.WeixinPay.Notify",
                 "Plugins/PaymentWeixinPay/Notify",
                 new { controller = "PaymentWeixinPay", action = "Notify" }
                 
            );

            //JsApi
            routeBuilder.MapRoute("Plugin.Payments.WeixinPay.JsApi",
                 "Plugins/PaymentWeixinPay/JsApi",
                 new { controller = "PaymentWeixinPay", action = "JsApi" }
                 
            );

            //H5Api
            routeBuilder.MapRoute("Plugin.Payments.WeixinPay.H5Api",
                 "Plugins/PaymentWeixinPay/H5Api",
                 new { controller = "PaymentWeixinPay", action = "H5Api" }
                 
            );

            routeBuilder.MapRoute("Plugin.Payments.WeixinPay.PayResult",
                "Plugins/PaymentWeixinPay/PayResult",
                new { controller = "PaymentWeixinPay", action = "PayResult" }
           );

            // Native 1
            routeBuilder.MapRoute("Plugin.Payments.WeixinPay.ProductPayCode",
                "Plugins/PaymentWeixinPay/ProductPayCode",
                new { controller = "PaymentWeixinPay", action = "ProductPayCode" }
           );
            //Native 2
            routeBuilder.MapRoute("Plugin.Payments.WeixinPay.Native2",
              "Plugins/PaymentWeixinPay/Native2",
              new { controller = "PaymentWeixinPay", action = "Native2" }
         );


            routeBuilder.MapRoute("Plugin.Payments.WeixinPay.Order",
              "Plugins/PaymentWeixinPay/Order",
              new { controller = "PaymentWeixinPay", action = "Order" }
           );


         

            routeBuilder.MapRoute("Plugin.Payments.WeixinPay.Index",
             "Plugins/PaymentWeixinPay/Index",
             new { controller = "PaymentWeixinPay", action = "Index" }
          );


            routeBuilder.MapRoute("Plugin.Payments.WeixinPay.OrderQuery",
            "Plugins/PaymentWeixinPay/OrderQuery",
            new { controller = "PaymentWeixinPay", action = "OrderQuery" }
         );

        }

        #endregion

        #region Properties

        public int Priority
        {
            get
            {
                return 0;
            }
        }

        #endregion
    }
}
