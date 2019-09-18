using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.LoginInfo;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Shipping;
using Nop.Web.Factories;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Security;

namespace Nop.Web.Controllers
{
    public partial class OrderController : BasePublicController
    {
        #region Fields

        private readonly IOrderModelFactory _orderModelFactory;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IPdfService _pdfService;
        private readonly IShipmentService _shipmentService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly IPictureService _pictureService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        #endregion

        #region Ctor

        public OrderController(IOrderModelFactory orderModelFactory,
            IOrderProcessingService orderProcessingService, 
            IOrderService orderService, 
            IPaymentService paymentService, 
            IPdfService pdfService,
            IShipmentService shipmentService, 
            IWebHelper webHelper,
            IWorkContext workContext,
            ICustomerActivityService customerActivityService,
            ICustomerService customerService,
            IPictureService pictureService,
            ILocalizationService localizationService,
            RewardPointsSettings rewardPointsSettings)
        {
            _orderModelFactory = orderModelFactory;
            _orderProcessingService = orderProcessingService;
            _orderService = orderService;
            _paymentService = paymentService;
            _pdfService = pdfService;
            _shipmentService = shipmentService;
            _webHelper = webHelper;
            _workContext = workContext;
            _pictureService = pictureService;
            _rewardPointsSettings = rewardPointsSettings;
            _customerService = customerService;
            _customerActivityService = customerActivityService;
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        //My account / Orders
        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult CustomerOrders()
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
                return Challenge();
            var model = _orderModelFactory.PrepareCustomerOrderListModel();
            return View(model);
        }
        //My account / Orders / Cancel recurring order
        [HttpPost, ActionName("CustomerOrders")]
        [PublicAntiForgery]
        [FormValueRequired(FormValueRequirement.StartsWith, "cancelRecurringPayment")]
        public virtual IActionResult CancelRecurringPayment(IFormCollection form)
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
                return Challenge();

            //get recurring payment identifier
            var recurringPaymentId = 0;
            foreach (var formValue in form.Keys)
                if (formValue.StartsWith("cancelRecurringPayment", StringComparison.InvariantCultureIgnoreCase))
                    recurringPaymentId = Convert.ToInt32(formValue.Substring("cancelRecurringPayment".Length));

            var recurringPayment = _orderService.GetRecurringPaymentById(recurringPaymentId);
            if (recurringPayment == null)
            {
                return RedirectToRoute("CustomerOrders");
            }

            if (_orderProcessingService.CanCancelRecurringPayment(_workContext.CurrentCustomer, recurringPayment))
            {
                var errors = _orderProcessingService.CancelRecurringPayment(recurringPayment);

                var model = _orderModelFactory.PrepareCustomerOrderListModel();
                model.RecurringPaymentErrors = errors;

                return View(model);
            }

            return RedirectToRoute("CustomerOrders");
        }
        //My account / Orders / Retry last recurring order
        [HttpPost, ActionName("CustomerOrders")]
        [PublicAntiForgery]
        [FormValueRequired(FormValueRequirement.StartsWith, "retryLastPayment")]
        public virtual IActionResult RetryLastRecurringPayment(IFormCollection form)
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
                return Challenge();

            //get recurring payment identifier
            var recurringPaymentId = 0;
            if (!form.Keys.Any(formValue => formValue.StartsWith("retryLastPayment", StringComparison.InvariantCultureIgnoreCase) &&
                int.TryParse(formValue.Substring(formValue.IndexOf('_') + 1), out recurringPaymentId)))
            {
                return RedirectToRoute("CustomerOrders");
            }

            var recurringPayment = _orderService.GetRecurringPaymentById(recurringPaymentId);
            if (recurringPayment == null)
                return RedirectToRoute("CustomerOrders");

            if (!_orderProcessingService.CanRetryLastRecurringPayment(_workContext.CurrentCustomer, recurringPayment))
                return RedirectToRoute("CustomerOrders");

            var errors = _orderProcessingService.ProcessNextRecurringPayment(recurringPayment);
            var model = _orderModelFactory.PrepareCustomerOrderListModel();
            model.RecurringPaymentErrors = errors.ToList();

            return View(model);
        }
        //My account / Reward points
        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult CustomerRewardPoints(int? pageNumber)
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
                return Challenge();

            if (!_rewardPointsSettings.Enabled)
                return RedirectToRoute("CustomerInfo");

            var model = _orderModelFactory.PrepareCustomerRewardPoints(pageNumber);
            return View(model);
        }
        //My account / Order details page
        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult Details(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null || order.Deleted || _workContext.CurrentCustomer.Id != order.CustomerId)
                return Challenge();

            var model = _orderModelFactory.PrepareOrderDetailsModel(order);
            return View(model);
        }
        //My account / Order details page / Print
        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult PrintOrderDetails(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null || order.Deleted || _workContext.CurrentCustomer.Id != order.CustomerId)
                return Challenge();

            var model = _orderModelFactory.PrepareOrderDetailsModel(order);
            model.PrintMode = true;

            return View("Details", model);
        }
        //My account / Order details page / PDF invoice
        public virtual IActionResult GetPdfInvoice(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null || order.Deleted || _workContext.CurrentCustomer.Id != order.CustomerId)
                return Challenge();

            var orders = new List<Order>();
            orders.Add(order);
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                _pdfService.PrintOrdersToPdf(stream, orders, _workContext.WorkingLanguage.Id);
                bytes = stream.ToArray();
            }
            return File(bytes, MimeTypes.ApplicationPdf, $"order_{order.Id}.pdf");
        }
        //My account / Order details page / re-order
        public virtual IActionResult ReOrder(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null || order.Deleted || _workContext.CurrentCustomer.Id != order.CustomerId)
                return Challenge();

            _orderProcessingService.ReOrder(order);
            return RedirectToRoute("ShoppingCart");
        }
        //My account / Order details page / Complete payment
        [HttpPost, ActionName("Details")]
        [PublicAntiForgery]
        [FormValueRequired("repost-payment")]
        public virtual IActionResult RePostPayment(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null || order.Deleted || _workContext.CurrentCustomer.Id != order.CustomerId)
                return Challenge();

            if (!_paymentService.CanRePostProcessPayment(order))
                return RedirectToRoute("OrderDetails", new { orderId = orderId });

            var postProcessPaymentRequest = new PostProcessPaymentRequest
            {
                Order = order
            };
            _paymentService.PostProcessPayment(postProcessPaymentRequest);

            if (_webHelper.IsRequestBeingRedirected || _webHelper.IsPostBeingDone)
            {
                //redirection or POST has been done in PostProcessPayment
                return Content("Redirected");
            }
            //if no redirection has been done (to a third-party payment page)
            //theoretically it's not possible
            return RedirectToRoute("OrderDetails", new { orderId = orderId });
        }
        //My account / Order details page / Shipment details page
        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult ShipmentDetails(int shipmentId)
        {
            var shipment = _shipmentService.GetShipmentById(shipmentId);
            if (shipment == null)
                return Challenge();
            var order = shipment.Order;
            if (order == null || order.Deleted || _workContext.CurrentCustomer.Id != order.CustomerId)
                return Challenge();
            var model = _orderModelFactory.PrepareShipmentDetailsModel(shipment);
            return View(model);
        }
        public virtual IActionResult TakeDetails(int orderId, string token, string qs_clientid)
        {


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
            if (!apitetoken.ClientId.Equals(qs_clientid))
            {
                return Json(new
                {
                    code = -1,
                    msg = "设备地址发生变化，请重新发起支付",
                    data = false
                });
            }
            var order = _orderService.GetOrderById(orderId);
            if (order == null || order.Deleted || apitetoken.ID != order.CustomerId.ToString())
            {
                return Json(new
                {
                    code = -1,
                    msg = "订单获取失败：原因订单不存在或不是本人订单",
                    data = new { }
                });
            }
            var imgurl = string.Empty;
            var model = _orderModelFactory.PrepareOrderDetailsModel(order);
            var orderItem = order.OrderItems.FirstOrDefault();
            if(orderItem != null)
            {
                
                var defaultProductPicture = _pictureService.GetPicturesByProductId(orderItem.ProductId, 1).FirstOrDefault();

                if (defaultProductPicture != null)
                {
                    imgurl = _pictureService.GetPictureUrl(defaultProductPicture);
                }
            }
            var vendors = order.OrderItems.Select(x =>string.Join(",", x.Product.ProductManufacturers.Select(v => v.Manufacturer.Name).ToList())).FirstOrDefault();
            return Json(new {
                 code = 0,
                 msg = "订单获取成功",
                data = new {
                    id  = model.Id,
                    status=(int)order.PaymentStatus,
                    total =order.OrderTotal,
                    Imgurl = string.IsNullOrEmpty(imgurl) ? "http://arbookresouce.73data.cn/book/img/sy_img_02.png" : imgurl,
                    goodsName =string.Join(",", order.OrderItems.Select(x=>x.Product.Name).ToList()),
                    Vendor = vendors,
                    InviteCode = order.InviteCode,
                    paymentMethod = model.PaymentMethod,
                }
            });
        }
        public virtual IActionResult GetList(string token, string qs_clientid)
        {
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
            if (!apitetoken.ClientId.Equals(qs_clientid))
            {
                return Json(new
                {
                    code = -1,
                    msg = "设备地址发生变化，请重新发起支付",
                    data = false
                });
            }
            var cid = 0;
            Int32.TryParse(apitetoken.ID, out cid);
            if (cid < 1)
            {
                return Json(new
                {
                    code = -1,
                    msg = "token 授权失败；客户不存在",
                    data = false
                });
            }
            try
            {
                var customer = _customerService.GetCustomerById(cid);
                if (customer == null)
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "用户授权失败；客户不存在",
                        data = new
                        {
                        }
                    });
                }
                var orders = _orderService.SearchOrders(0, 0, cid);
                return Json(new {
                    code = 0,
                    msg = "成功获取数据",
                    data = orders.ToList().Select(x =>
                    {
                        var orderitem = x.OrderItems.FirstOrDefault();

                        var product = orderitem.Product;

                        var imgUrl = string.Empty;


                        var imgurl = string.Empty;
                        var defaultProductPicture = _pictureService.GetPicturesByProductId(orderitem.Product.Id, 1).FirstOrDefault();

                        if (defaultProductPicture != null)
                        {
                            imgurl = _pictureService.GetPictureUrl(defaultProductPicture);
                        }
                        string statusdesc = string.Empty;


                        switch (x.OrderStatus)
                        {
                            case OrderStatus.Pending:
                                statusdesc = "待支付";
                                break;
                            case OrderStatus.Processing:
                                statusdesc = "处理中";
                                break;
                            case OrderStatus.Cancelled:
                                statusdesc = "已取消";
                                break;
                            case OrderStatus.Complete:
                                statusdesc = "已完成";
                                break;
                                
                        }

                        return new {
                            orderID = x.Id,
                            BookId = product.Id,
                            BookName = product.Name,
                            BookPicUrl = imgurl,
                            Publisher = "",
                            PayPrice = x.OrderTotal,
                            OrderTime = x.CreatedOnUtc.ToLocalTime(),
                            PayState = x.PaymentStatus == Core.Domain.Payments.PaymentStatus.Paid,
                            PayStateDesc = statusdesc,      






                            OrderNo = x.OrderGuid,
                            PayWay = "微信支付",
                            SerialNumber = x.OrderGuid
                        };

                    }).ToList()

                });
                    
                    
                    
                
            }
            catch (Exception ex)
            {
                return Json(new
                {

                    code = 0,
                    msg = ex.Message,
                    data = new List<string>()
                });
            }               
        }
        [HttpPost]
        public virtual IActionResult CancelOrder(string token, string qs_clientid,int orderid)
        {   
            //try to get an order with the specified id
            var order = _orderService.GetOrderById(orderid);
            if (order == null)
            {
                return Json(new
                {
                    code = -1,
                    msg = "订单不存在",
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
            if (!apitetoken.ClientId.Equals(qs_clientid))
            {
                return Json(new
                {
                    code = -1,
                    msg = "设备地址发生变化，",
                    data = false
                });
            }
            try
            {
                _orderProcessingService.CancelOrder(order, true);
                LogEditOrder(order.Id);
                return Json(new
                {
                    code = 0,
                    msg = "取消订单成功",
                    data = true
                });
                //prepare model
                //var model = _orderModelFactory.PrepareOrderModel(null, order);
                //return View(model);
            }
            catch (Exception exc)
            {
                return Json(new
                {
                    code = -1,
                    msg = "系统异常:" + exc.Message,
                    data = false
                });
                //prepare model
                //var model = _orderModelFactory.PrepareOrderModel(null, order);

                //_notificationService.ErrorNotification(exc);
                //return View(model);
            }

           
        }
        protected virtual void LogEditOrder(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            _customerActivityService.InsertActivity("EditOrder",
                string.Format(_localizationService.GetResource("ActivityLog.EditOrder"), order.CustomOrderNumber), order);
        }
        #endregion
    }
}