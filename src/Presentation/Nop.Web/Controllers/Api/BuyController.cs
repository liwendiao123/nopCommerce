using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.LoginInfo;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Shipping;
using Nop.Web.Areas.Admin.Factories;

namespace Nop.Web.Controllers.Api
{
    public class BuyController : Controller
    {
        #region Fields
        private readonly IOrderModelFactory _orderModelFactory;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IPdfService _pdfService;
        protected readonly ICustomerService _customerService;
        private readonly IShipmentService _shipmentService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly RewardPointsSettings _rewardPointsSettings;
        #endregion

        public BuyController(
            IOrderModelFactory orderModelFactory,
            IOrderProcessingService orderProcessingService,
            IOrderService orderService,
            IPaymentService paymentService,
            IPdfService pdfService,
            IShipmentService shipmentService,
            IWebHelper webHelper,
            IWorkContext workContext,
            ICustomerService customerService,
        RewardPointsSettings rewardPointsSettings
            )
        {
            _orderModelFactory = orderModelFactory;
            _orderProcessingService = orderProcessingService;
            _orderService = orderService;
            _paymentService = paymentService;
            _pdfService = pdfService;
            _shipmentService = shipmentService;
            _webHelper = webHelper;
            _workContext = workContext;
            _rewardPointsSettings = rewardPointsSettings;
            _customerService = customerService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Book(string token, string pid,string qs_clientid,int paymethod)
        {              
            //下单凭证
            ///var token = Request.Form["token"];
            if (string.IsNullOrEmpty(token))
            {
                return Json(new
                {
                    code = -1,
                    msg = "未登录",
                    data = false
                });
            }
            var productid = 0;
            Int32.TryParse(pid, out productid);
            if (productid < 1)
            {
                return Json(new
                {
                    code = -1,
                    msg = "请指定商品",
                    data = false
                });
            }
            var apitetoken = new AccountToken();
            apitetoken = AccountToken.Deserialize(token);
            if (apitetoken == null)
            {
                return Json(new
                {
                    code = -1,
                    msg = "token 授权失败",
                    data = false
                });
            }
            if(!apitetoken.ClientId.Equals(qs_clientid))
            {
                return Json(new
                {
                    code = -1,
                    msg = "设备地址发生变化，请重新发起支付",
                    data = false
                });
            }
            var cid = 0;
           Int32.TryParse(apitetoken.ID ?? "0", out cid);
           var customer =  _customerService.GetCustomerById(cid);
            if (customer == null)
            {
                return Json(new
                {
                    code = -1,
                    msg = "token 授权失败:用户不存在",
                    data = false
                });
            }       
            return Json(new
            {
                code = -1,
                msg = "获取成功",
                data = new {
                }
            });
        }
    }
}