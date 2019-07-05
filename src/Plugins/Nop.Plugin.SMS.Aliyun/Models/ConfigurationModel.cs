using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.Models;

namespace Nop.Plugin.SMS.Aliyun.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.SMS.Aliyun.Settings.Fields.Enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// AccessKeyID
        /// </summary>
        [NopResourceDisplayName("Plugins.SMS.Aliyun.Settings.Fields.AccessKeyID")]
        public string AccessKeyID { get; set; }
     
        /// <summary>
        /// AccessKeySecret
        /// </summary>
        [NopResourceDisplayName("Plugins.SMS.Aliyun.Settings.Fields.AccessKeySecret")]
        public string AccessKeySecret { get; set; }

        /// <summary>
        /// 短信签名
        /// </summary>
        [NopResourceDisplayName("Plugins.SMS.Aliyun.Settings.Fields.SignName")]
        public string SignName { get; set; }

        /// <summary>
        /// 短信产品
        /// </summary>
        [NopResourceDisplayName("Plugins.SMS.Aliyun.Settings.Fields.Product")]
        public string Product { get; set; }

        /// <summary>
        /// 服务的域名：dysmsapi.aliyuncs.com
        /// </summary>
        [NopResourceDisplayName("Plugins.SMS.Aliyun.Settings.Fields.Domain")]
        public string Domain { get; set; }

        /// <summary>
        /// 地区：cn-hangzhou  中国杭州
        /// </summary>
        [NopResourceDisplayName("Plugins.SMS.Aliyun.Settings.Fields.RegionId")]
        public string RegionId { get; set; }
 
        /// <summary>
        ///  终结点名 cn-hangzhou
        /// </summary>
        [NopResourceDisplayName("Plugins.SMS.Aliyun.Settings.Fields.EndpointName")]
        public string EndpointName { get; set; }
    }
}