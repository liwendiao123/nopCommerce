using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Nop.Core.Domain.AIBookModel;
using Nop.Core.Domain.Discounts;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.TableOfContent
{

    /// <summary>
    /// Category (for caching)
    /// </summary>
    [Serializable]
    //Entity Framework will assume that any class that inherits from a POCO class that is mapped to a table on the database requires a Discriminator column
    //That's why we have to add [NotMapped] as an attribute of the derived class.
    [NotMapped]
    public partial class BookDirModel : BaseNopEntityModel, ILocalizedModel<BookDirLocalizedModel>, IStoreMappingSupportedModel, IAclSupportedModel, IDiscountSupportedModel
        
    {


        #region Ctor 构造器

        public BookDirModel()
        {

            Locales = new List<BookDirLocalizedModel>();
            BookList =new List<SelectListItem>();
            ParentBookDir = new List<SelectListItem>();
            SelectedCustomerRoleIds = new List<int>();
            AvailableCustomerRoles = new List<SelectListItem>();
            AvailableCategories = new List<SelectListItem>();
            AvailableDiscounts = new List<SelectListItem>();
            AvailableStores = new List<SelectListItem>();
            SelectedStoreIds = new List<int>();

        }

        #endregion


        #region Properties

        private ICollection<AiBookModel> _aiBookModels;



        public int CategoryTemplateId { get; set; }

        /// <summary>
        /// 课本目录名称
        /// </summary>
        [Required]
        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.Name")]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.Description")]
        public string Description { get; set; }


        /// <summary>
        /// 设置媒体关键字
        /// </summary>
        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }


        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.MetaTitle")]
        public string MetaTitle { get; set; }


        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.ParentBookDirId")]
        public int ParentBookDirId { get; set; }


        
        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.CategryID")]
        public int CategryID { get; set; }

        /// <summary>
        /// 书籍ID
        /// </summary>
        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.BookID")]
        public int BookID { get; set; }



        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        [UIHint("Picture")]
        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.PictureID")]
        public int PictureId { get; set; }

        /// <summary>
        /// 获取、设置页面大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customers can select the page size
        /// </summary>
        public bool AllowCustomersToSelectPageSize { get; set; }

        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.IsLastNode")]
        /// <summary>
        ///  是否为知识点
        /// </summary>
        public bool IsLastNode { get; set; }
        #endregion


        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.PriceRanges")]
        public string PriceRanges { get; set; }


        /// <summary>
        /// Gets or sets the available customer selectable page size options
        /// </summary>
        public string PageSizeOptions { get; set; }

        /// <summary>
        ///获取或设置一个值，该值指示实体是否受权限设置控制
        /// </summary>
        public bool SubjectToAcl { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the entity is limited/restricted to certain stores
        /// </summary>
        public bool LimitedToStores { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        /// 
        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance update
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; }


        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.Breadcrumb")]
        public string Breadcrumb { get; internal set; }


        public string SeName { get; internal set; }



        //ACL (customer roles)
   
        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.AclCustomerRoles")]
        public IList<int> SelectedCustomerRoleIds { get; set; }



        public IList<SelectListItem> AvailableCustomerRoles { get; set; }

        //store mapping

        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.LimitedToStores")]
        public IList<int> SelectedStoreIds { get; set; }


   
        public IList<SelectListItem> AvailableStores { get; set; }



        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.AvailableCategories")]
        public IList<SelectListItem> AvailableCategories { get; set; }



        [JsonIgnore]
        //discounts
        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.Discounts")]
        public IList<int> SelectedDiscountIds { get; set; }


        [JsonIgnore]
        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.AvailableDiscounts")]
        public IList<SelectListItem> AvailableDiscounts { get; set; }



        public IList<BookDirLocalizedModel> Locales { get; set; }


        public IList<SelectListItem> BookList { get; set; }


        public IList<SelectListItem> ParentBookDir { get; set; }

        public BookDirSearchModel CategoryProductSearchModel { get; set; }


        //public virtual ICollection<AiBookModel> AiBookModels
        //{
        //    get => _aiBookModels ?? (_aiBookModels = new List<AiBookModel>());
        //    set => _aiBookModels = value;
        //}

       
        public new Dictionary<string, object> CustomProperties { get; set; }
        /// <summary>
        /// Gets or sets the collection of applied discounts
        /// </summary>
        ///  public virtual IList<Discount> AppliedDiscounts => DiscountBookDirMappings.Select(mapping => mapping.Discount).ToList();
    }


    public partial class BookDirLocalizedModel: ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.Description")]
        public string Description { get; set; }

        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.MetaKeywords")]
        public string MetaKeywords { get;  set; }

        [NopResourceDisplayName("Admin.AiBook.BookDir.Fields.MetaDescription")]
        public string MetaDescription { get;  set; }
    }
}
