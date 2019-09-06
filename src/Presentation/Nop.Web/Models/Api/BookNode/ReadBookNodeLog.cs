using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api.BookNode
{

    /// <summary>
    /// 阅读知识点记录
    /// </summary>
    public class ReadBookNodeLog
    {

        /// <summary>
        /// 记录Id
        /// </summary>
        public string guid { get; set; }
        /// <summary>
        /// 知识点ID
        /// </summary>
        public string booknodeId { get; set; }
        /// <summary>
        /// 知识点名称
        /// </summary>
        public string bookNodeName { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string customerId { get; set; }
        public string customerName { get; set; }
        public string starttime { get; set; }
        public string endTime { get; set; }
        public string readTime { get; set; }
        public string isvalid { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime { get; set; }
    }
}
