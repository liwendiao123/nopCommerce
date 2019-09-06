using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.Catalog;

namespace Nop.Core.Domain.Customers
{

    /// <summary>
    /// 兑换码
    /// </summary>
   public class CustomerOrderCode:BaseEntity
    {


        /// <summary>
        /// 客户ID
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 兑换码
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 有效期限
        /// </summary>
        public long ValidDays { get; set; }
        /// <summary>
        /// 代码类型
        /// </summary>
        public int CodeType { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActived { get; set; }
        /// <summary>
        /// 指定购买者ID
        /// </summary>
        public int OwenerId { get; set; }
        /// <summary>
        /// 指定购买者电话号码
        /// </summary>
        public string Phone { get; set; }

       public virtual Customer Customer { get; set; }
    
        public virtual Product Product { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}



