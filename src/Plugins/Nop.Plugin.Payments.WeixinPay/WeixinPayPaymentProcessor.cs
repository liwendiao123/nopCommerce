using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;

using Nop.Core.Domain.Directory;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Payments;
using Nop.Services.Directory;
using Nop.Web.Framework;
//using Nop.Plugin.Payments.WeixinPay.Controllers;
//using Senparc.Weixin.MP.TenPayLibV3;
using Nop.Services.Plugins;
using Senparc.Weixin.TenPay.V3;

namespace Nop.Plugin.Payments.WeixinPay
{
    /// <summary>
    /// WeixinPay payment processor
    /// </summary>
    public class WeixinPayPaymentProcessor : BasePlugin, IPaymentMethod
    {

        private static TenPayV3Info _tenPayV3Info;

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
        
       

        #region Constants

        #endregion

        #region Fields

        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IStoreContext _storeContext;
        private readonly WeixinPayPaymentSettings _weixinPayPaymentSettings;
        
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public WeixinPayPaymentProcessor(
            ISettingService settingService, 
            IWebHelper webHelper,
            IStoreContext storeContext,
            WeixinPayPaymentSettings weixinPayPaymentSettings,

            ICurrencyService currencyService,
            CurrencySettings currencySettings,
            IConfiguration configuration,
            ILocalizationService localizationService
          )
        {
            this._settingService = settingService;
            this._webHelper = webHelper;
            this._storeContext = storeContext;
            this._weixinPayPaymentSettings = weixinPayPaymentSettings;

            this._currencyService = currencyService;
            this._currencySettings = currencySettings;

            this._localizationService = localizationService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Gets MD5 hash
        /// </summary>
        /// <param name="input">Input</param>
        /// <param name="inputCharset">Input charset</param>
        /// <returns>Result</returns>
        internal string GetMD5(string input, string inputCharset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(inputCharset).GetBytes(input));
            var sb = new StringBuilder(32);

            foreach (var b in t)
            {
                sb.AppendFormat("{0:X}", b);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Create URL
        /// </summary>
        /// <param name="para">Para</param>
        /// <param name="inputCharset">Input charset</param>
        /// <param name="key">Key</param>
        /// <returns>Result</returns>
        private string CreatUrl(string[] para, string inputCharset, string key)
        {
            Array.Sort(para, StringComparer.InvariantCulture);

            int i;
            var prestr = new StringBuilder();

            for (i = 0; i < para.Length; i++)
            {
                prestr.Append(para[i]);

                if (i < para.Length - 1)
                {
                    prestr.Append("&");
                }
            }

            prestr.Append(key);

            var sign = GetMD5(prestr.ToString(), inputCharset);

            return sign;
        }

        /// <summary>
        /// Gets HTTP
        /// </summary>
        /// <param name="strUrl">Url</param>
        /// <param name="timeout">Timeout</param>
        /// <returns>Result</returns>
        internal string GetHttp(string strUrl, int timeout)
        {
            var strResult = string.Empty;

            try
            {
                var myReq = (HttpWebRequest)WebRequest.Create(strUrl);

                myReq.Timeout = timeout;

                var httpWResp = (HttpWebResponse)myReq.GetResponse();
                var myStream = httpWResp.GetResponseStream();
                if (myStream != null)
                {
                    using (var sr = new StreamReader(myStream, Encoding.Default))
                    {
                        var strBuilder = new StringBuilder();

                        while (-1 != sr.Peek())
                        {
                            strBuilder.Append(sr.ReadLine());
                        }

                        strResult = strBuilder.ToString();
                    }
                }
            }
            catch (Exception exc)
            {
                strResult = string.Format("Error: {0}", exc.Message);
            }

            return strResult;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Process a payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult {NewPaymentStatus = PaymentStatus.Pending};

            return result;
        }


        /// <summary>
        /// Post process payment (used by payment gateways that require redirecting to a third-party URL)
        /// </summary>
        /// <param name="postProcessPaymentRequest">Payment info required for an order processing</param>
        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
           
            var url = _webHelper.GetStoreHost(_webHelper.IsCurrentConnectionSecured()) + "Plugins/PaymentWeixinPay/Order";

            var post = new RemotePost
            {
                FormName = "wexinpaysubmit",
                Url = url,
                Method = "GET"
            };

            post.Add("orderId", postProcessPaymentRequest.Order.Id.ToString());
           
            post.Post();
            
        }

        /// <summary>
        /// Returns a value indicating whether payment method should be hidden during checkout
        /// </summary>
        /// <param name="cart">Shoping cart</param>
        /// <returns>true - hide; false - display.</returns>
        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            //you can put any logic here
            //for example, hide this payment method if all products in the cart are downloadable
            //or hide this payment method if current customer is from certain country
            return false;
        }

        /// <summary>
        /// Gets additional handling fee
        /// </summary>
        /// <param name="cart">Shoping cart</param>
        /// <returns>Additional handling fee</returns>
        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            return _weixinPayPaymentSettings.AdditionalFee;
        }
        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="capturePaymentRequest">Capture payment request</param>
        /// <returns>Capture payment result</returns>
        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            var result = new CapturePaymentResult();

            result.AddError("Capture method not supported");

            return result;
        }

        /// <summary>
        /// Refunds a payment
        /// </summary>
        /// <param name="refundPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            //initial
            var result = new RefundPaymentResult();

            var currencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;

            decimal amountToRefund = refundPaymentRequest.AmountToRefund;
            decimal orderTotal = refundPaymentRequest.Order.OrderTotal;

            if (currencyCode != "CNY" )
            {
                Currency currency = _currencyService.GetCurrencyByCode("CNY");
                amountToRefund = _currencyService.ConvertFromPrimaryStoreCurrency(refundPaymentRequest.AmountToRefund, currency);
                orderTotal = _currencyService.ConvertFromPrimaryStoreCurrency(refundPaymentRequest.Order.OrderTotal, currency);
            }

            // result.AddError("Refund method not supported");

            string nonceStr = TenPayV3Util.GetNoncestr();
            Senparc.Weixin.TenPay.V3.RequestHandler packageReqHandler = new RequestHandler(null);

           
            packageReqHandler.SetParameter("appid", _weixinPayPaymentSettings.TenPayV3_AppId);		  
            packageReqHandler.SetParameter("mch_id", _weixinPayPaymentSettings.TenPayV3_MchId);		 
            packageReqHandler.SetParameter("out_trade_no", refundPaymentRequest.Order.Id.ToString());                
            packageReqHandler.SetParameter("out_refund_no", refundPaymentRequest.Order.OrderGuid.ToString());                
            packageReqHandler.SetParameter("total_fee", (orderTotal * 100).ToString().Split('.')[0] );               
            packageReqHandler.SetParameter("refund_fee", (amountToRefund * 100).ToString().Split('.')[0]);            
            packageReqHandler.SetParameter("op_user_id", _weixinPayPaymentSettings.TenPayV3_MchId);  
            packageReqHandler.SetParameter("nonce_str", nonceStr);              
            string sign = packageReqHandler.CreateMd5Sign("key", _weixinPayPaymentSettings.TenPayV3_Key);
            packageReqHandler.SetParameter("sign", sign);	               
           
            string data = packageReqHandler.ParseXML();

            
            string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";

            string cert = _weixinPayPaymentSettings.TenPayV3_CertPath;            
            string password = _weixinPayPaymentSettings.TenPayV3_CertPassword;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
         
            X509Certificate2 cer = new X509Certificate2(cert, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

            #region 
            HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webrequest.ClientCertificates.Add(cer);
            webrequest.Method = "post";

            byte[] postdatabyte = Encoding.UTF8.GetBytes(data);
            webrequest.ContentLength = postdatabyte.Length;
            Stream stream;
            stream = webrequest.GetRequestStream();
            stream.Write(postdatabyte, 0, postdatabyte.Length);
            stream.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)webrequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();
            #endregion

            var res = XDocument.Parse(responseContent);
            string return_code = res.Element("xml").Element("return_code").Value;
            string return_msg = res.Element("xml").Element("return_msg").Value;


            if (return_code == "SUCCESS")
            {
                if (refundPaymentRequest.IsPartialRefund)
                    result.NewPaymentStatus = PaymentStatus.PartiallyRefunded;
                else
                    result.NewPaymentStatus = PaymentStatus.Refunded;
            }
            else {

                result.AddError(string.Format("WeixinPay error: {0} ({1})",return_msg, return_code));

            }

            return result;
        }

        /// <summary>
        /// Voids a payment
        /// </summary>
        /// <param name="voidPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            var result = new VoidPaymentResult();

            result.AddError("Void method not supported");

            return result;
        }

        /// <summary>
        /// Process recurring payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult();

            result.AddError("Recurring payment not supported");

            return result;
        }

        /// <summary>
        /// Cancels a recurring payment
        /// </summary>
        /// <param name="cancelPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            var result = new CancelRecurringPaymentResult();

            result.AddError("Recurring payment not supported");

            return result;
        }

        /// <summary>
        /// Gets a value indicating whether customers can complete a payment after order is placed but not completed (for redirection payment methods)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Result</returns>
        public bool CanRePostProcessPayment(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            //WeixinPay is the redirection payment method
            //It also validates whether order is also paid (after redirection) so customers will not be able to pay twice
            
            //payment status should be Pending
            if (order.PaymentStatus != PaymentStatus.Pending)
                return false;

            //let's ensure that at least 1 minute passed after order is placed
            return !((DateTime.UtcNow - order.CreatedOnUtc).TotalMinutes < 1);
        }


        /// <summary>
        /// Validate payment form
        /// </summary>
        /// <param name="form">The parsed form values</param>
        /// <returns>List of validating errors</returns>
        public IList<string> ValidatePaymentForm(IFormCollection form)
        {
            return new List<string>();
        }

        /// <summary>
        /// Get payment information
        /// </summary>
        /// <param name="form">The parsed form values</param>
        /// <returns>Payment info holder</returns>
        public ProcessPaymentRequest GetPaymentInfo(IFormCollection form)
        {
            return new ProcessPaymentRequest();
        }


        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/PaymentWeixinPay/Configure";
        }

        

        public override void Install()
        {
            //settings
            var settings = new WeixinPayPaymentSettings()
            {

                TenPayV3_MchId = "1369537202",
                TenPayV3_Key = "Key",
                TenPayV3_AppId = "wx2071d206df519d97",
                TenPayV3_AppSecret = "9e61eb782446ea9cca0f81dcfb2d6821",
                TenPayV3_TenpayNotify = "http://api.qisankeji.com/Plugins/PaymentWeixinPay/Notify",
                TenPayV3_CertPath = @"c:\apiclient_cert.p12",
                TenPayV3_CertPassword = "qskj@123456"
            };
            _settingService.SaveSetting(settings);
            //locales
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.IsFinished", "Finished payment");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.IsFinished",  "已完成支付", "zh-CN");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.NotFinished", "Not finished payment");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.NotFinished",  "支付遇到问题，重新支付", "zh-CN");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.RedirectionTip", "You will be redirected to WeixinPay site to complete the order.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_MchId", "Mch Id:");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_MchId.Hint", "Mch Id.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_MchId", "Mch Id:", "zh-CN");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_Key", "Key:");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_Key.Hint", "Key.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_Key",  "Key:", "zh-CN");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_AppId", "App Id:");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_AppId.Hint", "App Id.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_AppId", "App Id:", "zh-CN");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_AppSecret", "App Secret:");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_AppSecret.Hint", "App Secret.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_AppSecret",  "App Secret:", "zh-CN");

            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_TenpayNotify", "Notify URL:");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_TenpayNotify.Hint", "Notify URL.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_TenpayNotify", "Notify URL:", "zh-CN");


            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_CertPath", "Cert Path:");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_CertPath.Hint", "Cert Path.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_CertPath", "Cert Path:", "zh-CN");


            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_CertPassword", "Cert Password:");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_CertPassword.Hint", "Cert Password.");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_CertPassword",  "Cert Password:", "zh-CN");


            base.Install();
        }

        public override void Uninstall()
        {
            //locales

            _localizationService.DeletePluginLocaleResource("Plugins.Payments.WeixinPay.IsFinished");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.WeixinPay.NotFinished");




            _localizationService.DeletePluginLocaleResource("Plugins.Payments.WeixinPay.SellerEmail.RedirectionTip");


            _localizationService.DeletePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_MchId");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_MchId.Hint");

            _localizationService.DeletePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_Key");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_Key.Hint");

            _localizationService.DeletePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_AppId");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_AppId.Hint");

            _localizationService.DeletePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_AppSecret");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_AppSecret.Hint");

            _localizationService.DeletePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_TenpayNotify");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_TenpayNotify.Hint");

            _localizationService.DeletePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_CertPath");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_CertPath.Hint");

            _localizationService.DeletePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_CertPassword");
            _localizationService.DeletePluginLocaleResource("Plugins.Payments.WeixinPay.TenPayV3_CertPassword.Hint");

            base.Uninstall();
        }

        public string GetPublicViewComponentName()
        {
            return "PaymentWeixinPay";
        }

        #endregion

        #region Properies

        /// <summary>
        /// Gets a value indicating whether capture is supported
        /// </summary>
        public bool SupportCapture
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether partial refund is supported
        /// </summary>
        public bool SupportPartiallyRefund
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether refund is supported
        /// </summary>
        public bool SupportRefund
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether void is supported
        /// </summary>
        public bool SupportVoid
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a recurring payment type of payment method
        /// </summary>
        public RecurringPaymentType RecurringPaymentType
        {
            get
            {
                return RecurringPaymentType.NotSupported;
            }
        }

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        public PaymentMethodType PaymentMethodType
        {
            get
            {
                return PaymentMethodType.Redirection;
            }
        }

        /// <summary>
        /// Gets a value indicating whether we should display a payment information page for this plugin
        /// </summary>
        public bool SkipPaymentInfo
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a payment method description that will be displayed on checkout pages in the public store
        /// </summary>
        public string PaymentMethodDescription
        {
            //return description of this payment method to be display on "payment method" checkout step. good practice is to make it localizable
            //for example, for a redirection payment method, description may be like this: "You will be redirected to PayPal site to complete the payment"
            get { return string.Empty; }
        }


        #endregion
    }
}
