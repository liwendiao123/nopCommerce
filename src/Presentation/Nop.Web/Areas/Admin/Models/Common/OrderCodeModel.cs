using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Common
{
    public class OrderCodeModel : BaseNopEntityModel
    {

        public OrderCodeModel()
        {
            CodTypeList = new List<SelectListItem>();
            OwnerCustomerList = new List<SelectListItem>();
        }

        public IList<SelectListItem> CodTypeList { get; set; }
        
        public IList<SelectListItem> OwnerCustomerList { get; set; }


        public IList<SelectListItem> ProductList { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        [NopResourceDisplayName("Admin.OrderCodeModel.Fields.CustomerId")]
        public int CustomerId { get; set; }
        [NopResourceDisplayName("Admin.OrderCodeModel.Fields.CodeType")]
        public int CodeType { get; set; }


        /// <summary>
        /// 类型描述
        /// </summary>
        [NopResourceDisplayName("Admin.OrderCodeModel.Fields.CodeTypeDesc")]
        public string CodeTypeDesc {
            get {

                if (CodeType == 0)
                {
                    return "兑换码";
                }
                else
                {
                    return "推荐码";
                }
                    

            }

        }

        /// <summary>
        /// 关联产品Id
        /// </summary>
        [NopResourceDisplayName("Admin.OrderCodeModel.Fields.ProductId")]
        public int ProductId { get; set; }


        [NopResourceDisplayName("Admin.OrderCodeModel.Fields.ProductName")]
        public string ProductName { get; set; }

        [NopResourceDisplayName("Admin.OrderCodeModel.Fields.CustomerName")]
        public string CustomerName { get; set; }
        /// <summary>
        /// 兑换码
        /// </summary>      
        [NopResourceDisplayName("Admin.OrderCodeModel.Fields.OrderCode")]
        public string OrderCode { get; set; }
        /// <summary>
        /// 有效期限
        /// </summary>     
        [NopResourceDisplayName("Admin.OrderCodeModel.Fields.ValidDays")]
        public long ValidDays { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        /// 
        [NopResourceDisplayName("Admin.OrderCodeModel.Fields.IsValid")]
        public bool IsValid { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        /// 
        [NopResourceDisplayName("Admin.OrderCodeModel.Fields.IsActived")]
        public bool IsActived { get; set; }
        [NopResourceDisplayName("Admin.OrderCodeModel.Fields.ActivedDesc")]
        public string ActivedDesc {

            get
            {
                if (this.IsActived)
                {
                    return "已使用";
                }
                else
                {
                    return "未使用";
                }
            }
        }
        [NopResourceDisplayName("Admin.OrderCodeModel.Fields.ExpireTime")]
        public string ExpireTime {
            get
            {
                return this.CreateTime.AddDays(ValidDays).ToString("yyyy-MM-dd HH:mm");
            }

        }
        /// <summary>
        /// 购买者名字
        /// </summary>
        [NopResourceDisplayName("Admin.OrderCodeModel.Fields.OwnerName")]
        public string OwnerName { get; set; }
        /// <summary>
        /// 指定购买者ID
        /// </summary>
        [NopResourceDisplayName("Admin.OrderCodeModel.Fields.OwnerId")]
        public int OwnerId { get; set; }
        /// <summary>
        /// 指定购买者电话号码
        /// </summary>

       // [DataType(DataType.PhoneNumber)]
        [NopResourceDisplayName("Admin.OrderCodeModel.Fields.Phone")]
        public string Phone { get; set; }


        public DateTime CreateTime { get; set; }

    }
}
