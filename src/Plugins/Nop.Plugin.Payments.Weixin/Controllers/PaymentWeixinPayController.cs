using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Drawing.Imaging;
using System.Collections.Specialized;
using System.Drawing;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;


using ZXing;
using ZXing.Common;

using Nop.Core;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Directory;

using Nop.Services.Directory;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Security;

using Nop.Services.Messages;

using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework;

using Nop.Plugin.Payments.WeixinPay.Models;

using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.MP.TenPayLibV3;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.Containers;


using System.Xml.Linq;
using ZXing.QrCode;

namespace Nop.Plugin.Payments.WeixinPay.Controllers
{
    public class PaymentWeixinPayController : BasePaymentController
    {
        #region Fields

        private readonly ISettingService _settingService;
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly ILogger _logger;
        private readonly ILocalizationService _localizationService;
        private readonly WeixinPayPaymentSettings _weixinPayPaymentSettings;
        private readonly PaymentSettings _paymentSettings;
        private readonly IWebHelper _webHelper;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;

        private readonly IOptions<SenparcWeixinSetting> _senparcWeixinSettings;
        private IConfiguration _configuration;

        private static TenPayV3Info _tenPayV3Info;

        private readonly IPermissionService _permissionService;

        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly INotificationService _notificationService;

        private readonly IPaymentPluginManager _paymentPluginManager;
        #endregion

        #region Ctor
        public PaymentWeixinPayController(ISettingService settingService,
            IPaymentService paymentService,
            IOrderService orderService,
            IOrderProcessingService orderProcessingService,
            ILogger logger,
            ILocalizationService localizationService,
            WeixinPayPaymentSettings weixinPayPaymentSettings,
            PaymentSettings paymentSettings,
             ICurrencyService currencyService,
            CurrencySettings currencySettings,
            IWebHelper webHelper,
            IOptions<SenparcWeixinSetting> senparcWeixinSetting,
            IConfiguration configuration,
            IPermissionService permissionService,
            IHostingEnvironment env,
            ICustomerActivityService customerActivityService,
        IHttpContextAccessor contextAccessor,
            INotificationService notificationService,
             IPaymentPluginManager paymentPluginManager
           )
        {
            this._settingService = settingService;
            this._paymentService = paymentService;
            this._orderService = orderService;
            this._orderProcessingService = orderProcessingService;
            this._logger = logger;
            this._localizationService = localizationService;
            this._weixinPayPaymentSettings = weixinPayPaymentSettings;
            this._paymentSettings = paymentSettings;
            this._webHelper = webHelper;
            this._currencyService = currencyService;
            this._currencySettings = currencySettings;
            this._senparcWeixinSettings = senparcWeixinSetting;
            this._configuration = configuration;
            _customerActivityService = customerActivityService;
            this._permissionService = permissionService;
            this._env = env;
            this._contextAccessor = contextAccessor;

            this._notificationService = notificationService;

            this._paymentPluginManager = paymentPluginManager;
        }

        #endregion

        #region Methods

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }


        /*
        public static TenPayV3Info TenPayV3Info
        {
            get
            {
                if (_tenPayV3Info == null)
                {
                    _tenPayV3Info =
                        TenPayV3InfoCollection.Data[Senparc.Weixin.Config.DefaultSenparcWeixinSetting.TenPayV3_MchId];
                }
                return _tenPayV3Info;
            }
        }
       */

        private static NameValueCollection ToNameValueCollection<tValue>(IDictionary<string, tValue> dictionary)
        {
            var collection = new NameValueCollection();
            foreach (var pair in dictionary)
                collection.Add(pair.Key, pair.Value.ToString());
            return collection;
        }


        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {

            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            var model = new ConfigurationModel
            {

                TenPayV3_MchId = _weixinPayPaymentSettings.TenPayV3_MchId,
                TenPayV3_Key = _weixinPayPaymentSettings.TenPayV3_Key,
                TenPayV3_AppId = _weixinPayPaymentSettings.TenPayV3_AppId,
                TenPayV3_AppSecret = _weixinPayPaymentSettings.TenPayV3_AppSecret,
                TenPayV3_TenpayNotify = _weixinPayPaymentSettings.TenPayV3_TenpayNotify,
                TenPayV3_CertPath = _weixinPayPaymentSettings.TenPayV3_CertPath,
                TenPayV3_CertPassword = _weixinPayPaymentSettings.TenPayV3_CertPassword

            };

            return View("~/Plugins/Payments.WeixinPay/Views/PaymentWeixinPay/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        [AdminAntiForgery]
        [Area(AreaNames.Admin)]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Configure();

            /*
            Senparc.Weixin.Config.DefaultSenparcWeixinSetting = _senparcWeixinSettings.Value;

            Senparc.Weixin.Config.DefaultSenparcWeixinSetting.TenPayV3_AppId = model.TenPayV3_AppId;
            Senparc.Weixin.Config.DefaultSenparcWeixinSetting.TenPayV3_AppSecret = model.TenPayV3_AppSecret;
            Senparc.Weixin.Config.DefaultSenparcWeixinSetting.TenPayV3_MchId = model.TenPayV3_MchId;
            Senparc.Weixin.Config.DefaultSenparcWeixinSetting.TenPayV3_Key = model.TenPayV3_Key;
            Senparc.Weixin.Config.DefaultSenparcWeixinSetting.TenPayV3_TenpayNotify = model.TenPayV3_TenpayNotify;
            */
            //save settings
            //添加Senparc.Weixin配置文件（内容可以根据需要对应修改）
            var services = new ServiceCollection();
            services.Configure<SenparcWeixinSetting>(_configuration.GetSection("SenparcWeixinSetting")).Configure<SenparcWeixinSetting>(

                s => {
                    s.TenPayV3_MchId = model.TenPayV3_MchId;
                    s.TenPayV3_Key = model.TenPayV3_Key;
                    s.TenPayV3_AppId = model.TenPayV3_AppId;
                    s.TenPayV3_AppSecret = model.TenPayV3_AppSecret;
                    s.TenPayV3_TenpayNotify = model.TenPayV3_TenpayNotify;

                }
                );

            var tenPayV3Info = new TenPayV3Info(model.TenPayV3_AppId, model.TenPayV3_AppSecret, model.TenPayV3_MchId, model.TenPayV3_Key, model.TenPayV3_TenpayNotify);
            TenPayV3InfoCollection.Register(tenPayV3Info);//微信V3（新版）

            _weixinPayPaymentSettings.TenPayV3_MchId = model.TenPayV3_MchId;
            _weixinPayPaymentSettings.TenPayV3_Key = model.TenPayV3_Key;
            _weixinPayPaymentSettings.TenPayV3_AppId = model.TenPayV3_AppId;
            _weixinPayPaymentSettings.TenPayV3_AppSecret = model.TenPayV3_AppSecret;
            _weixinPayPaymentSettings.TenPayV3_TenpayNotify = model.TenPayV3_TenpayNotify;

            _weixinPayPaymentSettings.TenPayV3_CertPath = model.TenPayV3_CertPath;
            _weixinPayPaymentSettings.TenPayV3_CertPassword = model.TenPayV3_CertPassword;


            _weixinPayPaymentSettings.AdditionalFee = model.AdditionalFee;

            _settingService.SaveSetting(_weixinPayPaymentSettings);

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        public IActionResult Notify()
        {

           // _logger.Information("微信扫码支付结果通知---11111111");

            //_customerActivityService.InsertActivity("UpdateOrder",
            //   _localizationService.GetResource("ActivityLog.UpdateOrder"), new Order());
            //_customerActivityService.InsertActivity("微信扫码支付 - 模式二", "支付回调", new Order());

            var processor = _paymentPluginManager.LoadPluginBySystemName("Payments.WeixinPay") as WeixinPayPaymentProcessor;

            if (processor == null
                 || !_paymentPluginManager.IsPluginActive(processor))
                throw new NopException("WeixinPay module cannot be loaded");

           

            Nop.Plugin.Payments.WeixinPay.Common.ResponseHandler  resHandler = new Nop.Plugin.Payments.WeixinPay.Common.ResponseHandler(_contextAccessor.HttpContext);

            string return_code = resHandler.GetParameter("return_code");
            string return_msg = resHandler.GetParameter("return_msg");

            string res = null;

            resHandler.SetKey(_weixinPayPaymentSettings.TenPayV3_Key);


           // _logger.Information("微信扫码支付结果通知---222222");
            //验证请求是否从微信发过来（安全）
            if (resHandler.IsTenpaySign())
            {
                res = "success";

             //   _logger.Information("微信扫码支付结果通知---33333333");
                string out_trade_no = resHandler.GetParameter("out_trade_no");

                int orderId;
                if (int.TryParse(out_trade_no, out orderId))
                {
                    var order = _orderService.GetOrderById(orderId);

                //    _customerActivityService.InsertActivity("微信扫码支付 - 模式二","支付成功", order);
                    if (order != null && _orderProcessingService.CanMarkOrderAsPaid(order))
                    {
                        try
                        {
                            _logger.Information("微信扫码支付成功");
                            _orderProcessingService.MarkOrderAsPaid(order);
                        }
                        catch (Exception e)
                        {
                        }
                    }

                }

                return Content(res);
                //正确的订单处理
            }
            else
            {
                _logger.Information("微信扫码支付结果通知---4444444444");
                res = "fail";
                //_customerActivityService.InsertActivity("微信扫码支付 - 模式二", return_msg, new Order() {

               // });
                return Content(res);
                //错误的订单处理
            }

            /*
            var fileStream = System.IO.File.OpenWrite(Server.MapPath("~/1.txt"));
            fileStream.Write(Encoding.Default.GetBytes(res), 0, Encoding.Default.GetByteCount(res));
            fileStream.Close();

            string xml = string.Format(@"<xml>
   <return_code><![CDATA[{0}]]></return_code>
   <return_msg><![CDATA[{1}]]></return_msg>
</xml>", return_code, return_msg);


            return Content(xml, "text/xml");

            */


        }
        /// <summary>
        ///获取code
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IActionResult Index(int orderId = 0)
        {
            var processor = _paymentPluginManager.LoadPluginBySystemName("Payments.WeixinPay") as WeixinPayPaymentProcessor;

            if (processor == null
                || !_paymentPluginManager.IsPluginActive(processor)
                || !processor.PluginDescriptor.Installed)
                throw new NopException("WeixinPay module cannot be loaded");

            var hostUrl = _webHelper.GetStoreHost(_webHelper.IsCurrentConnectionSecured());

            var returnUrl = string.Format(hostUrl + "Plugins/PaymentWeixinPay/JsApi");
            var state = string.Format("{0}|{1}", orderId, orderId);

            var url = OAuthApi.GetAuthorizeUrl(_weixinPayPaymentSettings.TenPayV3_AppId, returnUrl, state, OAuthScope.snsapi_base);

            return Redirect(url);
        }

        public IActionResult PayResult()
        {


            return View("~/Plugins/Payments.WeixinPay/Views/PaymentWeixinPay/PayNotifyUrl.cshtml");

        }


        public IActionResult H5Api(int orderId = 0)
        {

            try
            {
                //获取产品信息
                var order = new Order();

                if (orderId > 0)
                {
                    order = _orderService.GetOrderById(orderId);

                }
                else
                {

                    return Content("查找订单失败");

                }


                string timeStamp = "";
                string nonceStr = "";
                string paySign = "";

                string sp_billno = "";
                //当前时间 yyyyMMdd
                string date = DateTime.Now.ToString("yyyyMMdd");
                sp_billno = orderId.ToString();

                //创建支付应答对象
                RequestHandler packageReqHandler = new RequestHandler(null);
                //初始化
                packageReqHandler.Init();

                timeStamp = TenPayV3Util.GetTimestamp();
                nonceStr = TenPayV3Util.GetNoncestr();

                string body = "";
                foreach (var item in order.OrderItems)
                {
                    body = item.Product.Name + item.Quantity.ToString() + " ";

                }

                //设置package订单参数


                decimal totalFee = order.OrderTotal;
                var currencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;
                if (currencyCode != "CNY")
                {
                    Currency currency = _currencyService.GetCurrencyByCode("CNY");
                    totalFee = _currencyService.ConvertFromPrimaryStoreCurrency(totalFee, currency);
                }

                packageReqHandler.SetParameter("appid", _weixinPayPaymentSettings.TenPayV3_AppId);          //公众账号ID
                packageReqHandler.SetParameter("mch_id", _weixinPayPaymentSettings.TenPayV3_MchId);         //商户号
                packageReqHandler.SetParameter("nonce_str", nonceStr);                    //随机字符串
                packageReqHandler.SetParameter("body", body);    //商品信息
                packageReqHandler.SetParameter("out_trade_no", sp_billno);      //商家订单号
                packageReqHandler.SetParameter("total_fee", (totalFee * 100).ToString().Split('.')[0]);                    //商品金额,以分为单位(money * 100).ToString()

                packageReqHandler.SetParameter("spbill_create_ip", Request.HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString());   //用户的公网ip，不是商户服务器IP
                packageReqHandler.SetParameter("notify_url", _weixinPayPaymentSettings.TenPayV3_TenpayNotify);

                string strScene = "{ 'h5_info': { 'type':'Wap','wap_url': '" + _webHelper.GetStoreHost(_webHelper.IsCurrentConnectionSecured()) + " ','wap_name': '" + Request.Host.Value + " '}";

                packageReqHandler.SetParameter("trade_type", "MWEB");
                packageReqHandler.SetParameter("scene_info", strScene);


                string sign = packageReqHandler.CreateMd5Sign("key", _weixinPayPaymentSettings.TenPayV3_Key);
                packageReqHandler.SetParameter("sign", sign);                       //签名

                string data = packageReqHandler.ParseXML();

                var result = TenPayV3.Unifiedorder(data);


                var res = XDocument.Parse(result);


                if (res.Element("xml").Element("mweb_url") == null)
                {
                    throw new Exception(res.ToString().HtmlEncode());
                }
                string mwebUrl = res.Element("xml").Element("mweb_url").Value;

                mwebUrl = mwebUrl + "&redirect_url=" + _webHelper.GetStoreHost(_webHelper.IsCurrentConnectionSecured()) + "Plugins/PaymentWeixinPay/PayResult";

                return Redirect(mwebUrl);





            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                msg += "<br>" + ex.StackTrace;
                msg += "<br>==Source==<br>" + ex.Source;

                if (ex.InnerException != null)
                {
                    msg += "<br>===InnerException===<br>" + ex.InnerException.Message;
                }
                return Content(msg);
            }


        }

        /// <summary>
        /// 扫码支付（模式二）
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Native2(int orderId = 0,string token = "", string qs_clientId = "")
        {

            try
            {
              //  var pathtest = System.IO.Path.Combine(_env.WebRootPath, "images") + "\\" + orderId.ToString() + ".jpg";
                //获取产品信息
                var order = new Order();
                if (orderId > 0)
                {
                    order = _orderService.GetOrderById(orderId);
                }
                else
                {
                    return Content("查找订单失败");
                }
                string timeStamp = "";
                string nonceStr = "";
                string paySign = "";
                string sp_billno = "";
                //当前时间 yyyyMMdd
                string date = DateTime.Now.ToString("yyyyMMdd");
                sp_billno = orderId.ToString();
                //创建支付应答对象
                RequestHandler packageReqHandler = new RequestHandler(null);
                //初始化
                packageReqHandler.Init();
                timeStamp = TenPayV3Util.GetTimestamp();
                nonceStr = TenPayV3Util.GetNoncestr();
                string body = "";
                foreach (var item in order.OrderItems)
                {
                    body = item.Product.Name + item.Quantity.ToString() + " ";
                }
                //设置package订单参数
                decimal totalFee = order.OrderTotal;
                var currencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;
                if (currencyCode != "CNY")
                {
                    Currency currency = _currencyService.GetCurrencyByCode("CNY");
                    totalFee = _currencyService.ConvertFromPrimaryStoreCurrency(totalFee, currency);
                }
                  packageReqHandler.SetParameter("appid", _weixinPayPaymentSettings.TenPayV3_AppId);          //公众账号ID
                  packageReqHandler.SetParameter("mch_id", _weixinPayPaymentSettings.TenPayV3_MchId);         //商户号
                  packageReqHandler.SetParameter("nonce_str", nonceStr);                    //随机字符串
                  packageReqHandler.SetParameter("body", body);    //商品信息
                  packageReqHandler.SetParameter("out_trade_no", sp_billno);      //商家订单号
                  packageReqHandler.SetParameter("total_fee", (totalFee * 100).ToString().Split('.')[0]);                    //商品金额,以分为单位(money * 100).ToString()
                  packageReqHandler.SetParameter("spbill_create_ip", Request.HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString());   //用户的公网ip，不是商户服务器IP
                  packageReqHandler.SetParameter("notify_url", _weixinPayPaymentSettings.TenPayV3_TenpayNotify);
                  packageReqHandler.SetParameter("trade_type", "NATIVE");
                  string sign = packageReqHandler.CreateMd5Sign("key", _weixinPayPaymentSettings.TenPayV3_Key);
                  packageReqHandler.SetParameter("sign", sign);                       //签名
                  string data = packageReqHandler.ParseXML();                         
                  var result = TenPayV3.Unifiedorder(data);
                _customerActivityService.InsertActivity("微信扫码支付-模式二", data + "==========="+ result, order);
                //get code_url
                var  res  = XDocument.Parse(result);
                if (res.Element("xml").Element("code_url") == null)
                {
                    throw new Exception(res.ToString().HtmlEncode());
                }
                string codeUrl = res.Element("xml").Element("code_url").Value;
                
               
                //creeate barcode

                var bw = new BarcodeWriterPixelData
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions
                    {
                        Height = 600,
                        Width = 600,
                        Margin = 0
                    }
                };

                var pixelData = bw.Write(codeUrl);

                Bitmap bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

                var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                    pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }


                var path = System.IO.Path.Combine(_env.WebRootPath, "images") + "\\" + orderId.ToString() + ".jpg";

                bitmap.Save(path, ImageFormat.Jpeg);

                Byte[] b = System.IO.File.ReadAllBytes(path);   // You can use your own method over here.         


                //return File(b, "image/jpeg");

                if (!string.IsNullOrEmpty(token))
                {
                    return Json(new
                    {
                        code = 0,
                        msg = "获取数据成功",
                        data = new {
                            barcodeUrl = Request.Scheme + "://" + Request.Host + @"images/" + orderId.ToString() + ".jpg"
                        }

                    });
                }
                return PhysicalFile(path, "image/jpeg");

            }
            catch (Exception ex)
            {

                if (!string.IsNullOrEmpty(token))
                {
                    return Json(new {
                        code = -1,
                        msg = "获取数据失败",
                        data = new
                        {
                            barcodeUrl = @"http://api.qisankeji.com/images/14.jpg"
                        }

                    });
                }

                var msg = ex.Message;
                msg += "<br>" + ex.StackTrace;
                msg += "<br>==Source==<br>" + ex.Source;

                if (ex.InnerException != null)
                {
                    msg += "<br>===InnerException===<br>" + ex.InnerException.Message;
                }
                return Content(msg);
            }


        }


        /// <summary>
        /// 扫码支付（模式二）
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// 
        [Route("/api/getbarcode")]
        [HttpGet]
        public IActionResult NativePay(int orderId = 0, string token = "", string qs_clientId = "")
        {

            try
            {

                _customerActivityService.InsertActivity("微信扫码支付-模式二", token + "===========" + orderId, new Order());
                //var pathtest = System.IO.Path.Combine(_env.WebRootPath, "images") + "\\" + orderId.ToString() + ".jpg";
                //获取产品信息
                var order = new Order();
                if (orderId > 0)
                {
                    order = _orderService.GetOrderById(orderId);
                }
                else
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        return Json(new
                        {
                             code = -1,
                             msg = "订单不存在",
                             data = new {
                             }
                        });
                    }
                    return Content("查找订单失败");
                }
                string timeStamp = "";
                string nonceStr = "";
                string paySign = "";
                string sp_billno = "";
                //当前时间 yyyyMMdd
                string date = DateTime.Now.ToString("yyyyMMdd");
                sp_billno = orderId.ToString();
                //创建支付应答对象
                RequestHandler packageReqHandler = new RequestHandler(null);
                //初始化
                packageReqHandler.Init();
                timeStamp = TenPayV3Util.GetTimestamp();
                nonceStr = TenPayV3Util.GetNoncestr();
                string body = "";
                foreach (var item in order.OrderItems)
                {
                    body = item.Product.Name + item.Quantity.ToString() + " ";
                }
                //设置package订单参数
                decimal totalFee = order.OrderTotal;
                var currencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;
                if (currencyCode != "CNY")
                {
                    Currency currency = _currencyService.GetCurrencyByCode("CNY");
                    totalFee = _currencyService.ConvertFromPrimaryStoreCurrency(totalFee, currency);
                }
                packageReqHandler.SetParameter("appid", _weixinPayPaymentSettings.TenPayV3_AppId);          //公众账号ID
                packageReqHandler.SetParameter("mch_id", _weixinPayPaymentSettings.TenPayV3_MchId);         //商户号
                packageReqHandler.SetParameter("nonce_str", nonceStr);                    //随机字符串
                packageReqHandler.SetParameter("body", body);    //商品信息
                packageReqHandler.SetParameter("out_trade_no", sp_billno);      //商家订单号
                packageReqHandler.SetParameter("total_fee", (totalFee * 100).ToString().Split('.')[0]);                    //商品金额,以分为单位(money * 100).ToString()
                packageReqHandler.SetParameter("spbill_create_ip", Request.HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString());   //用户的公网ip，不是商户服务器IP
                packageReqHandler.SetParameter("notify_url", _weixinPayPaymentSettings.TenPayV3_TenpayNotify);
                packageReqHandler.SetParameter("trade_type", "NATIVE");
                string sign = packageReqHandler.CreateMd5Sign("key", _weixinPayPaymentSettings.TenPayV3_Key);
                packageReqHandler.SetParameter("sign", sign);                       //签名
                string data = packageReqHandler.ParseXML();
                var result = TenPayV3.Unifiedorder(data);
                _customerActivityService.InsertActivity("微信扫码支付-模式二", data + "===========" + result, order);
                //get code_url
                var res = XDocument.Parse(result);
                if (res.Element("xml").Element("code_url") == null)
                {
                    throw new Exception(res.ToString().HtmlEncode());
                }
                string codeUrl = res.Element("xml").Element("code_url").Value;


                //creeate barcode

                var bw = new BarcodeWriterPixelData
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions
                    {
                        Height = 600,
                        Width = 600,
                        Margin = 0
                    }
                };

                var pixelData = bw.Write(codeUrl);

                Bitmap bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

                var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                    pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }


                var path = System.IO.Path.Combine(_env.WebRootPath, "images") + "\\" + orderId.ToString() + ".jpg";

                bitmap.Save(path, ImageFormat.Jpeg);

                Byte[] b = System.IO.File.ReadAllBytes(path);   // You can use your own method over here.         

                //return File(b, "image/jpeg");
                if (!string.IsNullOrEmpty(token))
                {
                    return Json(new
                    {
                        code = 0,
                        msg = "获取数据成功",
                        data = new
                        {
                            barcodeUrl = Request.Scheme + "://" + Request.Host + @"/images/" + orderId.ToString() + ".jpg"
                        }
                    });
                }
                return PhysicalFile(path, "image/jpeg");

            }
            catch (Exception ex)
            {

                if (!string.IsNullOrEmpty(token))
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "获取数据失败",
                        data = new
                        {
                            barcodeUrl = @"http://api.qisankeji.com/images/14.jpg"
                        }

                    });
                }

                var msg = ex.Message;
                msg += "<br>" + ex.StackTrace;
                msg += "<br>==Source==<br>" + ex.Source;

                if (ex.InnerException != null)
                {
                    msg += "<br>===InnerException===<br>" + ex.InnerException.Message;
                }
                return Content(msg);
            }


        }


        public IActionResult Test2(int orderId = 0, string token = "",string qs_clientId = "")
        {
            return Json(new
            {
                code = 0,
                msg = "测试",
                data = new {
                    token = token,
                    orderId = orderId,
                    qs_clientId = qs_clientId
                }

            });
        }

        /// <summary>
        /// 调用JSSDK  支付
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>

        public IActionResult JsApi(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }

            if (!state.Contains("|"))
            {
                //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
                //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                return Content("验证失败！请从正规途径进入！1001");
            }
            try
            {
                //获取产品信息
                var stateData = state.Split('|');
                int orderId = 0;

                var order = new Order();

                if (int.TryParse(stateData[0], out orderId))
                {
                    order = _orderService.GetOrderById(orderId);

                }
                else {

                    return Content("查找订单失败");

                }

                //通过，用code换取access_token
                var openIdResult = OAuthApi.GetAccessToken(_weixinPayPaymentSettings.TenPayV3_AppId, _weixinPayPaymentSettings.TenPayV3_AppSecret, code);
                if (openIdResult.errcode != ReturnCode.请求成功)
                {
                    return Content("错误：" + openIdResult.errmsg);
                }

                string timeStamp = "";
                string nonceStr = "";
                string paySign = "";

                string sp_billno = "";
                //当前时间 yyyyMMdd
                string date = DateTime.Now.ToString("yyyyMMdd");
                sp_billno = orderId.ToString();

                //创建支付应答对象
                RequestHandler packageReqHandler = new RequestHandler(null);
                //初始化
                packageReqHandler.Init();

                timeStamp = TenPayV3Util.GetTimestamp();
                nonceStr = TenPayV3Util.GetNoncestr();

                string body = "";
                foreach (var item in order.OrderItems)
                {
                    body = item.Product.Name + item.Quantity.ToString() + " ";

                }

                //设置package订单参数


                decimal totalFee = order.OrderTotal;
                var currencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;
                if (currencyCode != "CNY")
                {
                    Currency currency = _currencyService.GetCurrencyByCode("CNY");
                    totalFee = _currencyService.ConvertFromPrimaryStoreCurrency(totalFee, currency);
                }

                packageReqHandler.SetParameter("appid", _weixinPayPaymentSettings.TenPayV3_AppId);          //公众账号ID
                packageReqHandler.SetParameter("mch_id", _weixinPayPaymentSettings.TenPayV3_MchId);         //商户号
                packageReqHandler.SetParameter("nonce_str", nonceStr);                    //随机字符串
                packageReqHandler.SetParameter("body", body);    //商品信息
                packageReqHandler.SetParameter("out_trade_no", sp_billno);      //商家订单号
                packageReqHandler.SetParameter("total_fee", (totalFee * 100).ToString().Split('.')[0]);                    //商品金额,以分为单位(money * 100).ToString()

                packageReqHandler.SetParameter("spbill_create_ip", Request.HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString());   //用户的公网ip，不是商户服务器IP
                packageReqHandler.SetParameter("notify_url", _weixinPayPaymentSettings.TenPayV3_TenpayNotify);

                packageReqHandler.SetParameter("trade_type", TenPayV3Type.JSAPI.ToString());                        //交易类型

                packageReqHandler.SetParameter("openid", openIdResult.openid);                      //用户的openId

                string sign = packageReqHandler.CreateMd5Sign("key", _weixinPayPaymentSettings.TenPayV3_Key);
                packageReqHandler.SetParameter("sign", sign);                       //签名

                string data = packageReqHandler.ParseXML();

                var result = TenPayV3.Unifiedorder(data);

                var res = XDocument.Parse(result);

                if (res.Element("xml").Element("prepay_id") == null)
                {
                    throw new Exception(res.ToString().HtmlEncode());
                }

                //throw new Exception(res.ToString().HtmlEncode());
                string prepayId = res.Element("xml").Element("prepay_id").Value;

                //设置支付参数
                RequestHandler paySignReqHandler = new RequestHandler(null);
                paySignReqHandler.SetParameter("appId", _weixinPayPaymentSettings.TenPayV3_AppId);
                paySignReqHandler.SetParameter("timeStamp", timeStamp);
                paySignReqHandler.SetParameter("nonceStr", nonceStr);
                paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
                paySignReqHandler.SetParameter("signType", "MD5");
                paySign = paySignReqHandler.CreateMd5Sign("key", _weixinPayPaymentSettings.TenPayV3_Key);

                ViewData["appId"] = _weixinPayPaymentSettings.TenPayV3_AppId;
                ViewData["timeStamp"] = timeStamp;
                ViewData["nonceStr"] = nonceStr;
                ViewData["package"] = string.Format("prepay_id={0}", prepayId);
                ViewData["paySign"] = paySign;
                ViewData["order"] = order;
                ViewData["orderId"] = order.Id.ToString();

                //得到ticket
                string ticket = JsApiTicketContainer.TryGetJsApiTicket(_weixinPayPaymentSettings.TenPayV3_AppId, _weixinPayPaymentSettings.TenPayV3_AppSecret);
                ////获取签名        
                var signature = JSSDKHelper.GetSignature(ticket, nonceStr, timeStamp, Request.GetDisplayUrl());
                ViewData["signature"] = signature;


                return View("~/Plugins/Payments.WeixinPay/Views/PaymentWeixinPay/JsApi.cshtml");



            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                msg += "<br>" + ex.StackTrace;
                msg += "<br>==Source==<br>" + ex.Source;

                if (ex.InnerException != null)
                {
                    msg += "<br>===InnerException===<br>" + ex.InnerException.Message;
                }
                return Content(msg);
            }
        }

        /// <summary>
        /// 扫码支付（模式一）
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// 
       [HttpGet]
        public IActionResult ProductPayCode(int orderId)
        {

            #region   old code

            /*

            try {

                var order = new Order();
                order = _orderService.GetOrderById(orderId);

                string timeStamp = "";
                string nonceStr = "";
                string paySign = "";

                string sp_billno = "";
                string date = DateTime.Now.ToString("yyyyMMdd");
                sp_billno = orderId.ToString();
                timeStamp = TenPayV3Util.GetTimestamp();
                nonceStr = TenPayV3Util.GetNoncestr();

                string body = "";
                foreach (var item in order.OrderItems)
                {
                    body = item.Product.Name + item.Quantity.ToString() + " ";
                }

                decimal totalFee = order.OrderTotal;
                var currencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;
                if (currencyCode != "CNY")
                {
                    Currency currency = _currencyService.GetCurrencyByCode("CNY");
                    totalFee = _currencyService.ConvertFromPrimaryStoreCurrency(totalFee, currency);

                }

              totalFee  = Convert.ToDecimal( (totalFee * 100).ToString().Split('.')[0]);

                var xmlDataInfo = new TenPayV3RequestData(TenPayV3Info.AppId,
                                                          TenPayV3Info.MchId,
                                                          body,
                                                          sp_billno,
                                                          totalFee,
                                                          Request.UserHostAddress,
                                                          TenPayV3Info.TenPayV3Notify,
                                                          TenPayV3Type.NATIVE,
                                                          "",
                                                          TenPayV3Info.Key,
                                                          nonceStr);

                var result = TenPayV3.Unifiedorder(xmlDataInfo);

                string codeUrl = result.code_url;

                BitMatrix bitMatrix;
                bitMatrix = new MultiFormatWriter().encode(codeUrl, BarcodeFormat.QR_CODE, 600, 600);
                BarcodeWriter bw = new BarcodeWriter();
                var ms = new MemoryStream();
                var bitmap = bw.Write(bitMatrix);
                bitmap.Save(ms, ImageFormat.Png);
                ms.WriteTo(Response.OutputStream);
                Response.ContentType = "image/png";
                return null;
            }
            catch(Exception ex)
            {

                var msg = ex.Message;
                msg += "<br>" + ex.StackTrace;
                msg += "<br>==Source==<br>" + ex.Source;

                if (ex.InnerException != null)
                {
                    msg += "<br>===InnerException===<br>" + ex.InnerException.Message;
                }
                return Content(msg);

            }
            */

#endregion



            var url = string.Format(_webHelper.GetStoreHost(_webHelper.IsCurrentConnectionSecured()) + "Plugins/PaymentWeixinPay/Index?orderId={0}&t={1}", orderId,
                     DateTime.Now.Ticks);


            var bw = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 600,
                    Width = 600,
                    Margin = 0
                }
            };

            var pixelData = bw.Write(url);

            Bitmap bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
           
            var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height),
            System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            try
            {
                // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
            
            
               var path = System.IO.Path.Combine( _env.WebRootPath,"images") +"\\"+ orderId.ToString() + ".jpg";

               bitmap.Save(path, ImageFormat.Jpeg);
            
               Byte[] b = System.IO.File.ReadAllBytes(path);   // You can use your own method over here.         
             //  return File(b, "image/jpeg");
              return PhysicalFile(path, "image/jpeg");
          }

        /// <summary>
        /// 处理Order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>     
        public IActionResult Order(int orderId)
        {
            var order = new Order();
            order = _orderService.GetOrderById(orderId);

            //判断是否正在微信端

            var headers = ToNameValueCollection(Request.Headers);
            
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            var mobileDetect = new MobileDetect.MobileDetect(headers, userAgent);

            if (userAgent != null
                && (userAgent.Contains("MicroMessenger") || userAgent.Contains("Windows Phone")))
            {
                //正在微信端，直接跳转到微信支付页面
                return RedirectToAction("Index", new { orderId = orderId });
            }
            else
            {
                if (mobileDetect.IsMobile(userAgent,headers))
                {
                    //Wap端打开
                    return RedirectToAction("H5Api", new { orderId = orderId });
                }
                else {
                    //在PC端打开，提供二维码扫描进行支付(模式一)
                    //return View("~/Plugins/Payments.WeixinPay/Views/PaymentWeixinPay/Order.cshtml", order);
                    //在PC端打开，提供二维码扫描进行支付(模式二)
                    return View("~/Plugins/Payments.WeixinPay/Views/PaymentWeixinPay/OrderNative.cshtml", order);
                }
            }
        }
        /// <summary>
        /// 订单查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult OrderQuery(int orderId)
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            RequestHandler packageReqHandler = new RequestHandler(null);

            //设置package订单参数
            packageReqHandler.SetParameter("appid", _weixinPayPaymentSettings.TenPayV3_AppId);		  //公众账号ID
            packageReqHandler.SetParameter("mch_id", _weixinPayPaymentSettings.TenPayV3_MchId);         //商户号
           // packageReqHandler.SetParameter("transaction_id", "");       //填入微信订单号 
            packageReqHandler.SetParameter("out_trade_no", orderId.ToString());         //填入商家订单号
           

            packageReqHandler.SetParameter("nonce_str", nonceStr);             //随机字符串
            string sign = packageReqHandler.CreateMd5Sign("key", _weixinPayPaymentSettings.TenPayV3_Key);
            packageReqHandler.SetParameter("sign", sign);	                    //签名

            string data = packageReqHandler.ParseXML();

            var result = TenPayV3.OrderQuery(data);
            var res = XDocument.Parse(result);

            string return_code = res.Element("xml").Element("return_code").Value;
            string result_code = "";
            string trade_state = "";

            if (return_code == "SUCCESS")
            {
                result_code = res.Element("xml").Element("result_code").Value;
                if (result_code == "SUCCESS")
                {

                    trade_state = res.Element("xml").Element("trade_state").Value;

                    return Json(new { resultType = "1", message = trade_state });
                }

            }


            return Json(new { resultType = "0",message="FAIL"});
        }
        #endregion
    }
}