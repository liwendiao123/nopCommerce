using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Departments
{
    public partial class DepartmentModel : BaseNopEntityModel, ILocalizedModel<DepartmentLocalizedModel
        >
    {

        public DepartmentModel()
        {
            Locales = new List<DepartmentLocalizedModel>();
        }
             

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
        public IList<DepartmentLocalizedModel> Locales { get;set; }
    }


    public class DepartmentLocalizedModel : ILocalizedLocaleModel
    {

        public int LanguageId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        /// 
        [NopResourceDisplayName("Admin.Department.Fields.Name")]
        public string Name { get; set; }
        /// <summary>
        /// 学校代号 唯一
        /// </summary>

        [NopResourceDisplayName("Admin.Department.Fields.VatCode")]
        public string VatCode { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        /// 
        [NopResourceDisplayName("Admin.Department.Fields.Tel")]
        public string Tel { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        /// 
        [NopResourceDisplayName("Admin.Department.Fields.Email")]
        public string Email { get; set; }
        /// <summary>
        /// 校园主页
        /// </summary>
        /// 
        [NopResourceDisplayName("Admin.Department.Fields.MainUrl")]
        public string MainUrl { get; set; }
        /// <summary>
        /// 学校封面
        /// </summary>
        /// 
        [NopResourceDisplayName("Admin.Department.Fields.ImgUrl")]
        public string ImgUrl { get; set; }
        /// <summary>
        /// 简要描述
        /// </summary>
        /// 
        [NopResourceDisplayName("Admin.Department.Fields.Desc")]
        public string Desc { get; set; }
        /// <summary>
        /// 校方联系人
        /// </summary>
        /// 
        [NopResourceDisplayName("Admin.Department.Fields.ContactPerson")]
        public string ContactPerson { get; set; }
    }


}
