using Nop.Core.Configuration;

namespace Nop.Plugin.SMS.Aliyun
{
    /// <summary>
    /// 阿里云短信配置参数，申请短信服务时，会拿到这些参数
    /// </summary>
    public class AliyunSmsSettings : ISettings
    {
        /// <summary>
        /// Gets or sets the value indicting whether this SMS provider is enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 授权账号
        /// </summary>
        public string AccessKeyID { get; set; }
        /// <summary>
        /// 授权码
        /// </summary>
        public string AccessKeySecret { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string SignName { get; set; }
        /// <summary>
        /// 短信产品 ：默认阿里大鱼 Dysmsapi
        /// </summary>
        public string Product { get; set; }
        /// <summary>
        /// 服务的域名：dysmsapi.aliyuncs.com
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 地区：cn-hangzhou  中国杭州
        /// </summary>
        public string RegionId { get; set; }

        /// <summary>
        /// 终结点名 cn-hangzhou
        /// </summary>
        public string EndpointName { get; set; }
    }
}
