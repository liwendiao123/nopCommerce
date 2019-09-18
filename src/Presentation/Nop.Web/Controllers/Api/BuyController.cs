using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Core.LoginInfo;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Shipping;
using Nop.Web.Areas.Admin.Factories;
using Nop.Core.Http.Extensions;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Newtonsoft.Json;
using Nop.Web.Models.Api.WebApiModel.ApiBuyBook;

namespace Nop.Web.Controllers.Api
{
    public class BuyController : BasePublicController
    {
        #region Fields
        private readonly IOrderModelFactory _orderModelFactory;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly Factories.ICheckoutModelFactory _checkoutModelFactory;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IPdfService _pdfService;
        protected readonly ICustomerService _customerService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IShipmentService _shipmentService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly IProductService _productService;
        private readonly IPictureService _pictureService;
        private readonly IStoreContext _storeContext;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly PaymentSettings _paymentSettings;
        private readonly OrderSettings _orderSettings;
        private readonly ICustomerOrderCodeService _customerOrderCodeService;

        #endregion

        public BuyController(
            IOrderModelFactory orderModelFactory,
            IProductService productService,
            IOrderProcessingService orderProcessingService,
            IOrderService orderService,
            IPaymentService paymentService,
            IPdfService pdfService,
            IShipmentService shipmentService,
            IWebHelper webHelper,
            IWorkContext workContext,
            IStoreContext storeContext,
            ICustomerOrderCodeService customerOrderCodeService,
            IShoppingCartService shoppingCartService,
            ICustomerService customerService,
              PaymentSettings paymentSettings,
                   OrderSettings orderSettings,
                   ICustomerActivityService customerActivityService,
                   ILocalizationService localizationService,
            IGenericAttributeService genericAttributeService,
            IPictureService pictureService,
        Factories.ICheckoutModelFactory checkoutModelFactory,
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
            _customerOrderCodeService = customerOrderCodeService;
            _rewardPointsSettings = rewardPointsSettings;
            _customerService = customerService;
            _storeContext = storeContext;
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _genericAttributeService = genericAttributeService;
            _checkoutModelFactory = checkoutModelFactory;
            _paymentSettings = paymentSettings;
            _orderSettings = orderSettings;
            _localizationService = localizationService;
            _pictureService = pictureService;
            _customerActivityService = customerActivityService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Book(string token, string pid,string qs_clientid,int paymethod,string inviteCode,string data)
        {

            
                 if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    var jsonrequest = JsonConvert.DeserializeObject<BuyBookJsonRequest>(data);
                    if (jsonrequest != null)
                    {
                        pid = jsonrequest.pid;
                        paymethod = jsonrequest.paymethod;
                        inviteCode = jsonrequest.inviteCode;
                        token = jsonrequest.token;
                        qs_clientid = jsonrequest.qs_clientId;
                    }
                }
                catch (Exception ex)
                {

                }

            }

            var tokenresult = ValidateToken(token, qs_clientid, _customerService);
            if (tokenresult == 1)
            {
                return Json(new
                {
                    code = -2,
                    msg = "该账号已在另一个地点登录,如不是本人操作，您的密码已经泄露,请及时修改密码！",
                    data = false
                });
            }
            if (tokenresult == 3)
            {
                return Json(new
                {
                    code = -3,
                    msg = "该账号已被禁用！",
                    data = false

                });
            }


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
            // 1.0获取用户信息
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
           var result =  _orderService.SearchOrders(0, 0, cid, productid).ToList();

            if (result != null && result.Count > 0)
            {      
                result = result.Where(x => x.OrderStatus == OrderStatus.Pending).OrderByDescending(x=>x.CreatedOnUtc).ToList();

                foreach (var item in result)
                {
                    try
                    {
                        _orderService.DeleteOrder(item);
                    }
                    catch (Exception ex)
                    {

                    }
                    
                }

            }


            //2.0获取产品
            var product = _productService.GetProductById(productid);

            if (product.Price <= 0)
            {
                return Json(new
                {
                    code = -1,
                    msg = "价格为零，无须支付",
                    data = new {
                    }
                });
            }

            string inviteCodeOk = string.Empty;

            var exitecode = _customerOrderCodeService.GetOrderCodeByCode(inviteCode);


            var codeExpireDate = DateTime.Now;
            if (exitecode != null && exitecode.CodeType == 1)
            {
                codeExpireDate = exitecode.CreateTime.AddDays(1).Date.AddDays(exitecode.ValidDays);
            }

            if (codeExpireDate > DateTime.Now)
            {
                inviteCodeOk = inviteCode;
            }

            //3.0添加购物车
            var shopcartresult =  _shoppingCartService.AddToCart(customer, 
                                                                  product,
                                            ShoppingCartType.ShoppingCart,
                                            _storeContext.CurrentStore.Id,null,product.Price,DateTime.Now,DateTime.Now.AddYears(10));
            var cart = _shoppingCartService.GetShoppingCart(customer, ShoppingCartType.ShoppingCart, _storeContext.CurrentStore.Id);
            //model
            // var model = _checkoutModelFactory.PrepareConfirmOrderModel(cart);
            var addr = new Core.Domain.Common.Address()
            {
                  Email = "novalidemail@163.com",
               
                  Company = "七三科技有限公司"
                   
            };
            //4.0生成订单 
            var order = new Order();
            order.CreatedOnUtc = DateTime.UtcNow;
            order.OrderGuid = new Guid();
            order.InviteCode = inviteCodeOk;
            order.PaymentStatus = PaymentStatus.Pending;
            order.ShippingStatus = ShippingStatus.NotYetShipped;
            order.OrderStatus = OrderStatus.Pending;
            customer.BillingAddress = order.BillingAddress == null ? addr : order.BillingAddress;
            customer.ShippingAddress = order.ShippingAddress == null ? addr : order.ShippingAddress;
            order.Customer = customer;
            ///支付方式 
            var paymentMethodModel = _checkoutModelFactory.PreparePaymentMethodModel(cart, 0);
            if (_paymentSettings.BypassPaymentMethodSelectionIfOnlyOne &&
                paymentMethodModel.PaymentMethods.Count == 1 && !paymentMethodModel.DisplayRewardPoints)
            {
                //if we have only one payment method and reward points are disabled or the current customer doesn't have any reward points
                //so customer doesn't have to choose a payment method
                _genericAttributeService.SaveAttribute(customer,
                    NopCustomerDefaults.SelectedPaymentMethodAttribute,
                    paymentMethodModel.PaymentMethods[0].PaymentMethodSystemName,
                    _storeContext.CurrentStore.Id);
                //  return RedirectToRoute("CheckoutPaymentInfo");
            }
            var isPaymentWorkflowRequired = _orderProcessingService.IsPaymentWorkflowRequired(cart, false);
            if (!isPaymentWorkflowRequired)
            {
                _genericAttributeService.SaveAttribute<string>(customer,
                    NopCustomerDefaults.SelectedPaymentMethodAttribute, null, _storeContext.CurrentStore.Id);
               // return RedirectToRoute("CheckoutPaymentInfo");
            }
            try
            {
                if (!IsMinimumOrderPlacementIntervalValid(customer))
                    throw new Exception(_localizationService.GetResource("Checkout.MinOrderPlacementInterval"));
                ///获取支付请求
                var processPaymentRequest = HttpContext.Session.Get<ProcessPaymentRequest>("OrderPaymentInfo");
                if (processPaymentRequest == null)
                {
                    //Check whether payment workflow is required
                    //if (_orderProcessingService.IsPaymentWorkflowRequired(cart))
                    //    return RedirectToRoute("CheckoutPaymentInfo");
                    processPaymentRequest = new ProcessPaymentRequest();
                }
                GenerateOrderGuid(processPaymentRequest);
                processPaymentRequest.StoreId = _storeContext.CurrentStore.Id;
                processPaymentRequest.CustomerId =customer.Id;
                processPaymentRequest.PaymentMethodSystemName = _genericAttributeService.GetAttribute<string>(customer,
                NopCustomerDefaults.SelectedPaymentMethodAttribute, _storeContext.CurrentStore.Id);
                HttpContext.Session.Set<ProcessPaymentRequest>("OrderPaymentInfo", processPaymentRequest);
                var placeOrderResult = _orderProcessingService.PlaceOrder(processPaymentRequest);
                if (placeOrderResult.Success)
                {
                    _customerActivityService.InsertActivity("AddNewOrder",
                        _localizationService.GetResource("ActivityLog.AddNewOrder"), order);
                    var orderid = placeOrderResult.PlacedOrder.Id;
                    var orderIdentifyId =  placeOrderResult.PlacedOrder.OrderGuid;
                    var orderTotal = placeOrderResult.PlacedOrder.OrderTotal;
                    return Json(new
                    {
                        code =0,
                        msg = "获取成功",
                        data = new
                        {
                           orderId = orderid,
                           orderIdentifyId = orderIdentifyId,
                           orderTotal = orderTotal,
                           invitecode = inviteCodeOk
                        }
                    });

                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = -1,
                    msg = ex.Message,
                    data = new {
                    }
                });
            }      
         //   _genericAttributeService.SaveAttribute<string>(_workContext.CurrentCustomer,
                //NopCustomerDefaults.SelectedPaymentMethodAttribute, null, _storeContext.CurrentStore.Id);
            ///加入购物车
          //  var cart = _shoppingCartService.GetShoppingCart(customer, ShoppingCartType.ShoppingCart);                   
            return Json(new
            {
                code = -1,
                msg = "获取成功",
                data = new {
                }
            });
        }
        protected virtual bool IsMinimumOrderPlacementIntervalValid(Customer customer)
        {
            //prevent 2 orders being placed within an X seconds time frame
            if (_orderSettings.MinimumOrderPlacementInterval == 0)
                return true;

            var lastOrder = _orderService.SearchOrders(storeId: _storeContext.CurrentStore.Id,
                customerId: _workContext.CurrentCustomer.Id, pageSize: 1)
                .FirstOrDefault();
            if (lastOrder == null)
                return true;

            var interval = DateTime.UtcNow - lastOrder.CreatedOnUtc;
            return interval.TotalSeconds > _orderSettings.MinimumOrderPlacementInterval;
        }
        /// <summary>
        /// Generate an order GUID
        /// </summary>
        /// <param name="processPaymentRequest">Process payment request</param>
        protected virtual void GenerateOrderGuid(ProcessPaymentRequest processPaymentRequest)
        {
            if (processPaymentRequest == null)
                return;
            //we should use the same GUID for multiple payment attempts
            //this way a payment gateway can prevent security issues such as credit card brute-force attacks
            //in order to avoid any possible limitations by payment gateway we reset GUID periodically
            var previousPaymentRequest = HttpContext.Session.Get<ProcessPaymentRequest>("OrderPaymentInfo");
            if (_paymentSettings.RegenerateOrderGuidInterval > 0 &&
                previousPaymentRequest != null &&
                previousPaymentRequest.OrderGuidGeneratedOnUtc.HasValue)
            {
                var interval = DateTime.UtcNow - previousPaymentRequest.OrderGuidGeneratedOnUtc.Value;
                if (interval.TotalSeconds < _paymentSettings.RegenerateOrderGuidInterval)
                {
                    processPaymentRequest.OrderGuid = previousPaymentRequest.OrderGuid;
                    processPaymentRequest.OrderGuidGeneratedOnUtc = previousPaymentRequest.OrderGuidGeneratedOnUtc;
                }
            }
            if (processPaymentRequest.OrderGuid == Guid.Empty)
            {
                processPaymentRequest.OrderGuid = Guid.NewGuid();
                processPaymentRequest.OrderGuidGeneratedOnUtc = DateTime.UtcNow;
            }
        }
        public IActionResult GetBarCode(int orderId = 0, string inviteCode = "" ,string token = "", string qs_clientId = "",string data = "")
        {

            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    var jsonrequest = JsonConvert.DeserializeObject<GetBarCodeJsonRequest>(data);
                    if (jsonrequest != null)
                    {
                        orderId = jsonrequest.orderId;
                        inviteCode = jsonrequest.inviteCode;
                        token = jsonrequest.token;
                        qs_clientId = jsonrequest.qs_clientId;
                    }
                }
                catch (Exception ex)
                {

                }

            }



            if (!string.IsNullOrEmpty(inviteCode))
            {
                string inviteCodeOk = string.Empty;

                var exitecode = _customerOrderCodeService.GetOrderCodeByCode(inviteCode);


                var codeExpireDate = DateTime.Now;
                if (exitecode != null && exitecode.CodeType == 1)
                {
                    codeExpireDate = exitecode.CreateTime.AddDays(1).Date.AddDays(exitecode.ValidDays);
                }

                if (codeExpireDate > DateTime.Now)
                {
                    inviteCodeOk = inviteCode;
                }

                if (!string.IsNullOrEmpty(inviteCode))
                {
                    try
                    {
                        var res = _orderService.GetOrderById(orderId);
                        if (res != null)
                        {
                            res.InviteCode = inviteCodeOk;

                            _orderService.UpdateOrder(res);
                        }
                    }
                    catch (Exception ex)
                    {

                    }             
                }


            }


            return Redirect(Request.Scheme + "://" + Request.Host + string.Format("/api/getbarcode?orderId={0}&token={1}&qs_clientId={2}",orderId,token,qs_clientId));
        }
    }
}