using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Stores;
using System.Linq;
using Nop.Core.Domain.AIBookModel;
using Nop.Core.Domain.Localization;
using Newtonsoft.Json;

namespace Nop.Core.Domain.TableOfContent
{

    /// <summary>
    /// 书籍目录
    /// </summary>
    public partial class BookDir : BaseEntity, ISlugSupported, IAclSupported, IStoreMappingSupported, ILocalizedEntity
    {


        private  ICollection<AiBookModel> _aiBookModels;

       //private ICollection<DiscountBookDirMapping> _discountCategoryMappings;
       public int BookID { get; set; }
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value of used category template identifier
        /// </summary>
       
       [JsonIgnore]
        public int CategoryTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the meta keywords
        /// </summary>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
        public string MetaTitle { get; set; }

        /// <summary>
        /// Gets or sets the parent category identifier
        /// </summary>
        public int ParentBookDirId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// Gets or sets the page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customers can select the page size
        /// </summary>
        public bool AllowCustomersToSelectPageSize { get; set; }

        /// <summary>
        /// Gets or sets the available customer selectable page size options
        /// </summary>
        public string PageSizeOptions { get; set; }

        /// <summary>
        /// Gets or sets the available price ranges
        /// </summary>
        public string PriceRanges { get; set; }
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
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance update
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; }

        public bool IsLastNode { get; set; }
   
        /// <summary>
        /// Gets or sets the discount-category mappings
        /// </summary>
        /// 
        [JsonIgnore]
        public virtual ICollection<AiBookModel> AiBookModels
        {
            get => _aiBookModels ?? (_aiBookModels = new List<AiBookModel>());
            set => _aiBookModels = value;
        }
    }
}
