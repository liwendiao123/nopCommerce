using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.WeixinPay
{
    public class WeixinPayPaymentSettings : ISettings
    {


        public  decimal AdditionalFee { get; set; }
        public  string TenPayV3_MchId { get; set; }
        public  string TenPayV3_Key { get; set; }
        public  string TenPayV3_AppId { get; set; }
        public  string TenPayV3_AppSecret { get; set; }
        public  string TenPayV3_TenpayNotify { get; set; }
        public  string  TenPayV3_CertPath { get; set; }
        public  string  TenPayV3_CertPassword { get; set; }


    }
}
