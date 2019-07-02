using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.AiBook
{

    /// <summary>
    /// 
    /// </summary>
    public partial class AiBookModelView : BaseNopEntityModel, ILocalizedModel<AiBookModelLocalizedModel>
    {

        #region  Field
        public IList<AiBookModelLocalizedModel> Locales { get; set; }
        #endregion

        #region Ctor 构造器
        public AiBookModelView()
        {
            Locales = new List<AiBookModelLocalizedModel>();
            AvailableCategories = new List<SelectListItem>();
            AvailableBooks = new List<SelectListItem>();
            AvailableBookDirs = new List<SelectListItem>();
        }
        #endregion


        #region 属性
        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.CateName")]
        public int CateId { get; set; }
        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.Book")]
        public int BookId { get; set; }
        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.BookDir")]
        public int BookDirId { get; set; }
        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.Name")]
        public string Name { get; set; }
        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.Desc")]
        public string Desc { get; set; }
        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.Unique")]
        public string UniqueId { get; set; }
        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.WebModelUrl")]
        public string WebModelUrl { get; set; }
        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.WebGltfUrl")]
        public string WebGltfUrl { get; set; }
        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.WebBinUrl")]
        public string WebBinUrl { get; set; }
        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.AbUrl")]
        public string AbUrl { get; set; }
        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.ImgUrl")]
        [UIHint("Picture")]
        public string ImgUrl { get; set; }
        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.StrJson")]
        public string StrJson { get; set; }

        public bool Published { get; set; }

        public bool Deleted { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime UpdatedOnUtc { get; set; }

        // public string 

        #endregion


        #region  附属集合
        /// <summary>
        /// 
        /// </summary>
        [NopResourceDisplayName("Admin.AiBook.BookModel.List.AvailableCategories")]
        public IList<SelectListItem> AvailableCategories { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [NopResourceDisplayName("Admin.AiBook.BookModel.List.AvailableBooks")]
        public IList<SelectListItem> AvailableBooks { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [NopResourceDisplayName("Admin.AiBook.BookModel.List.AvailableBookDirs")]
        public IList<SelectListItem> AvailableBookDirs { get; set; }
        #endregion
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class AiBookModelLocalizedModel : ILocalizedLocaleModel
    {

        public int LanguageId { get; set; }
        [NopResourceDisplayName("Admin.AiBook.AiBookModel.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.AiBook.AiBookModel.Fields.Desc")]
        public string Desc { get; set; }
    
        [NopResourceDisplayName("Admin.AiBook.AiBookModel.Fields.WebModelUrl")]
        public string WebModelUrl { get; set; }   
        [NopResourceDisplayName("Admin.AiBook.AiBookModel.Fields.WebGltfUrl")]
        public string WebGltfUrl { get; set; }
        [NopResourceDisplayName("Admin.AiBook.AiBookModel.Fields.WebBinUrl")]
        public string WebBinUrl { get; set; }
        [NopResourceDisplayName("Admin.AiBook.AiBookModel.Fields.AbUrl")]
        public string AbUrl { get; set; }    
        [NopResourceDisplayName("Admin.AiBook.AiBookModel.Fields.ImgUrl")]
        public string ImgUrl { get; set; }
        [NopResourceDisplayName("Admin.AiBook.AiBookModel.Fields.StrJson")]
        public string StrJson { get; set; }
    }
}
