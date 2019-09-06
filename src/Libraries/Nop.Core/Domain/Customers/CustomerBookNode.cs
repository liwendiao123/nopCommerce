using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.AIBookModel;
using Nop.Core.Domain.Catalog;

namespace Nop.Core.Domain.Customers
{

    /// <summary>
    /// 客户知识点目录
    /// </summary>
   public class CustomerBookNode:BaseEntity
    {
        public int CustomerId { get; set; }
        public int BookNodeId { get; set; }  
        /// <summary>
        /// 书籍
        /// </summary>
        public virtual AiBookModel Product { get; set; }
        /// <summary>
        /// 客户信息
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// 标识为已读
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// 是否加入收藏夹
        /// </summary>
        public bool IsFocus { get; set; }
        /// <summary>
        /// 以秒为单位
        /// </summary>
        public long ReadTime { get; set; }


        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
