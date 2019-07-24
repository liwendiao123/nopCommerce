using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Models.Api.BookNode;

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
            PrefabPathList = new List<SelectListItem>();
            BookNodeRoot = new BookNodeRoot();
            EventIdList = new List<SelectListItem>();
            ResourceList = new List<SelectListItem>();
            InitPrefabList();
            InitEventIdList();
        }
        #endregion
        private void InitPrefabList()
        {
            PrefabPathList.Add(new SelectListItem
            {
                 Value = @"K/Audio/CKAudio",
                  Text = @"K/Audio/CKAudio"
            });
            PrefabPathList.Add(new SelectListItem
            {
                Value= @"K/Button/Button",
                Text = @"K/Button/Button"
            });
            PrefabPathList.Add(new SelectListItem
            {
                Value = @"K/Camera/DefaultCamera",
                Text = @"K/Camera/DefaultCamera"
            });
            PrefabPathList.Add(new SelectListItem
            {
                Value = @"K/Camera/Camera",
                Text  = @"K/Camera/Camera"
            });
            PrefabPathList.Add(new SelectListItem
            {
                Value = @"K/Image/Image",
                Text = @"K/Image/Image"
            });
            PrefabPathList.Add(new SelectListItem
            {
                Value = @"K/Text/Text",
                Text = @"K/Text/Text"
            });
            PrefabPathList.Add(new SelectListItem
            {
                Value = @"K/Video/VideoPlayer",
                Text = @"K/Video/VideoPlayer"
            });
        }


        private void InitEventIdList()
        {
            EventIdList.Add(new SelectListItem
            {

                Value = "-1",
                 Text = "无效事件"

            });
        }

        #region 属性
        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.CateName")]
        public int CateId { get; set; }

        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.PrefabPath")]
        public string PrefabPath { get; set; }
        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.EventId")]
        public string EventId { get; set; }

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
        public string ImgUrl { get; set; }
        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.StrJson")]
        public string StrJson { get; set; }

        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.UnityJson")]
        public string UnityStrJson { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Published")]


     
        public bool Published { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Categories.Fields.Deleted")]
        public bool Deleted { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Categories.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.Active")]
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

        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.PrefabPathList")]
        /// <summary>
        /// 预制路径集合
        /// </summary>
        public IList<SelectListItem> PrefabPathList { get; set; }



        /// <summary>
        /// 事件ID集合
        /// </summary>
        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.EventIdList")]
        public IList<SelectListItem> EventIdList { get; set; }

        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.ResourceList")]
        public IList<SelectListItem> ResourceList { get; set; }
        //[NopResourceDisplayName("Admin.AiBook.BookNode.Fields.ButtonPrefabPathList")]
        //public IList<SelectListItem> ButtonPrefabPathList
        //{
        //    get;
        //    set;
        //}


        public BookNodeRoot BookNodeRoot { get; set; }
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
