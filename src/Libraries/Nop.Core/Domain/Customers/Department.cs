using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Seo;

namespace Nop.Core.Domain.Customers
{
   public  class Department : BaseEntity,ILocalizedEntity, ISlugSupported
    {

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 学校代号 唯一
        /// </summary>
        public string VatCode { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; }


        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 校园主页
        /// </summary>
        public string MainUrl { get; set; }


       /// <summary>
       /// 学校封面
       /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 简要描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 校方联系人
        /// </summary>
        public string ContactPerson { get; set; }

        /// <summary>
        /// 展示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer has been deleted
        /// </summary>
        public bool Deleted { get; set; }


        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }


        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; }
    }
}
