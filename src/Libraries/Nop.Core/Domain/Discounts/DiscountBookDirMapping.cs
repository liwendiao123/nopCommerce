using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.TableOfContent;

namespace Nop.Core.Domain.Discounts
{


    /// <summary>
    /// 目录折扣对应表
    /// </summary>
   public partial class DiscountBookDirMapping:BaseEntity
    {
        /// <summary>
        /// Gets or sets the discount identifier
        /// </summary>
        public string DiscountId { get; set; }
        /// <summary>
        /// Gets or sets the category identifier
        /// </summary>
        public string DiscountBookDirId { get; set; }
        /// <summary>
        /// 获取或设置折扣
        /// </summary>
        public virtual Discount Discount { get; set; }
        /// <summary>
        /// 书籍目录
        /// </summary>
        public virtual BookDir BookDir { get; set; }
    }
}
