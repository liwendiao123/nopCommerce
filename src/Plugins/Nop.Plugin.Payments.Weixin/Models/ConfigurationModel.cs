using Nop.Web.Framework;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.ModelBinding;


namespace Nop.Plugin.Payments.WeixinPay.Models
{
    public class ConfigurationModel : BaseNopModel
    {


        [NopResourceDisplayName("Plugins.Payments.WeixinPay.AdditionalFee")]
        public decimal AdditionalFee { get; set; }

        [NopResourceDisplayName("Plugins.Payments.WeixinPay.TenPayV3_MchId")]
        public string TenPayV3_MchId { get; set; }


        [NopResourceDisplayName("Plugins.Payments.WeixinPay.TenPayV3_Key")]
        public string TenPayV3_Key { get; set; }

        [NopResourceDisplayName("Plugins.Payments.WeixinPay.TenPayV3_AppId")]
        public string TenPayV3_AppId { get; set; }

        [NopResourceDisplayName("Plugins.Payments.WeixinPay.TenPayV3_AppSecret")]
        public string TenPayV3_AppSecret { get; set; }

        [NopResourceDisplayName("Plugins.Payments.WeixinPay.TenPayV3_TenpayNotify")]
        public string TenPayV3_TenpayNotify { get; set; }

        [NopResourceDisplayName("Plugins.Payments.WeixinPay.TenPayV3_URL")]
        public string TenPayV3_URL { get; set; }

        [NopResourceDisplayName("Plugins.Payments.WeixinPay.TenPayV3_CertPath")]
        public string TenPayV3_CertPath { get; set; }

        [NopResourceDisplayName("Plugins.Payments.WeixinPay.TenPayV3_CertPassword")]
        public string TenPayV3_CertPassword { get; set; }



    }

}