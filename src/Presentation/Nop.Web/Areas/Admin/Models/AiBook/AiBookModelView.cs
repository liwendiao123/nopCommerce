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
            BookNodeRoot = new BookNodeNewRoot();
          //  BookNodeNewRoot = new BookNodeNewRoot();
            EventIdList = new List<SelectListItem>();
            ResourceList = new List<SelectListItem>();
            InitPrefabList();
            InitEventIdList();
        }
        #endregion
        private void InitPrefabList()
        {
            #region  获取文本路径
            if (TextPrefebPath == null)
            {
                TextPrefebPath = new List<SelectListItem>();
            }
            TextPrefebPath.Add(new SelectListItem {
                Value = "K/Text/blackboard",
                 Text = "左下黑板文本预制"
            });
            TextPrefebPath.Add(new SelectListItem
            {
                Value = "K/Text/Center",
                Text = "居中对齐"
            });
            TextPrefebPath.Add(new SelectListItem
            {
                Value = "K/Text/CenterBottom",
                Text = "中下对齐"
            });
            TextPrefebPath.Add(new SelectListItem
            {
                Value = "K/Text/CenterTop",
                Text = "中上对齐"
            });
            TextPrefebPath.Add(new SelectListItem
            {
                Value = "K/Text/LeftBottom",
                Text = "左下对齐"
            });
            TextPrefebPath.Add(new SelectListItem
            {
                Value = "K/Text/LeftCenter",
                Text = "左中对齐"
            });
            TextPrefebPath.Add(new SelectListItem
            {
                Value = "K/Text/LeftTop",
                Text = "左上对齐"
            });
            TextPrefebPath.Add(new SelectListItem
            {
                Value = "K/Text/RightBottom",
                Text = "右下对齐"
            });
            TextPrefebPath.Add(new SelectListItem
            {
                Value = "K/Text/RightCenter",
                Text = "右中对齐"
            });
            TextPrefebPath.Add(new SelectListItem
            {
                Value = "K/Text/RightTop",
                Text = "右上对齐"
            });
            #endregion
            #region 初始化按钮预制路径
            if (ButtonPrefebPath == null)
            {
                ButtonPrefebPath = new List<SelectListItem>();
            }
      
            ButtonPrefebPath.Add(new SelectListItem
            {
                Value = "K/Button/Center",
                Text = "居中对齐"
            });
            ButtonPrefebPath.Add(new SelectListItem
            {
                Value = "K/Button/CenterBottom",
                Text = "中下对齐"
            });
            ButtonPrefebPath.Add(new SelectListItem
            {
                Value = "K/Button/CenterTop",
                Text = "中上对齐"
            });
            ButtonPrefebPath.Add(new SelectListItem
            {
                Value = "K/Button/LeftBottom",
                Text = "左下对齐"
            });
            ButtonPrefebPath.Add(new SelectListItem
            {
                Value = "K/Button/LeftCenter",
                Text = "左中对齐"
            });
            ButtonPrefebPath.Add(new SelectListItem
            {
                Value = "K/Button/LeftTop",
                Text = "左上对齐"
            });
            ButtonPrefebPath.Add(new SelectListItem
            {
                Value = "K/Button/RightBottom",
                Text = "右下对齐"
            });
            ButtonPrefebPath.Add(new SelectListItem
            {
                Value = "K/Button/RightCenter",
                Text = "右中对齐"
            });
            ButtonPrefebPath.Add(new SelectListItem
            {
                Value = "K/Button/RightTop",
                Text = "右上对齐"
            });
            #endregion
            #region 初始化图片预制路径
            if (ImgPrefebPath == null)
            {
                ImgPrefebPath = new List<SelectListItem>();
            }
            ImgPrefebPath.Add(new SelectListItem
            {
                Value = "K/Image/Center",
                Text = "居中对齐"
            });
            ImgPrefebPath.Add(new SelectListItem
            {
                Value = "K/Image/CenterBottom",
                Text = "中下对齐"
            });
            ImgPrefebPath.Add(new SelectListItem
            {
                Value = "K/Image/CenterTop",
                Text = "中上对齐"
            });
            ImgPrefebPath.Add(new SelectListItem
            {
                Value = "K/Image/LeftBottom",
                Text = "左下对齐"
            });
            ImgPrefebPath.Add(new SelectListItem
            {
                Value = "K/Image/LeftCenter",
                Text = "左中对齐"
            });
            ImgPrefebPath.Add(new SelectListItem
            {
                Value = "K/Image/LeftTop",
                Text = "左上对齐"
            });
            ImgPrefebPath.Add(new SelectListItem
            {
                Value = "K/Image/RightBottom",
                Text = "右下对齐"
            });
            ImgPrefebPath.Add(new SelectListItem
            {
                Value = "K/Image/RightCenter",
                Text = "右中对齐"
            });
            ImgPrefebPath.Add(new SelectListItem
            {
                Value = "K/Image/RightTop",
                Text = "右上对齐"
            });
            #endregion
            #region 初始化视频预制路径
            if (VideoPrefebPath == null)
            {
                VideoPrefebPath = new List<SelectListItem>();
            }
            VideoPrefebPath.Add(new SelectListItem
            {
                Value = "K/Video/Center",
                Text = "居中对齐"
            });
            VideoPrefebPath.Add(new SelectListItem
            {
                Value = "K/Video/CenterBottom",
                Text = "中下对齐"
            });
            VideoPrefebPath.Add(new SelectListItem
            {
                Value = "K/Video/CenterTop",
                Text = "中上对齐"
            });
            VideoPrefebPath.Add(new SelectListItem
            {
                Value = "K/Video/LeftBottom",
                Text = "左下对齐"
            });
            VideoPrefebPath.Add(new SelectListItem
            {
                Value = "K/Video/LeftCenter",
                Text = "左中对齐"
            });
            VideoPrefebPath.Add(new SelectListItem
            {
                Value = "K/Video/LeftTop",
                Text = "左上对齐"
            });
            VideoPrefebPath.Add(new SelectListItem
            {
                Value = "K/Video/RightBottom",
                Text = "右下对齐"
            });
            VideoPrefebPath.Add(new SelectListItem
            {
                Value = "K/Video/RightCenter",
                Text = "右中对齐"
            });
            VideoPrefebPath.Add(new SelectListItem
            {
                Value = "K/Video/RightTop",
                Text = "右上对齐"
            });
            #endregion

            #region  初始化音频路径 
            if (AudioPrefebPath == null)
            {
                AudioPrefebPath = new List<SelectListItem>();
            }
            AudioPrefebPath.Add(new SelectListItem
            {
                Value = "K/Audio/CKAudio",
                Text = "音源预制"
            });
            #endregion

            #region 初始化相机预制路径

            if (CameraPrefebPath == null)
            {
                CameraPrefebPath = new List<SelectListItem>();
            }
            CameraPrefebPath.Add(new SelectListItem
            {
                Value = "K/Camera/DefaultCamera",
                Text = "静态视角相机"
            });
            CameraPrefebPath.Add(new SelectListItem
            {
                Value = "K/Camera/Camera",
                Text = "动态视角相机"
            });
            #endregion

            #region

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

            #endregion
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



        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.UniqueID")]
        public string UniqueID { get; set; }


        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Published")]
        public bool Published { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Categories.Fields.Deleted")]
        public bool Deleted { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Categories.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.Active")]
        public bool Active { get; set; }

        /// <summary>
        /// 是否为特例
        /// </summary>
        public int ComplexLevel { get; set; }
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


        /// <summary>
        /// 文本预制路径
        /// </summary>
        public List<SelectListItem> TextPrefebPath { get; set; }


        /// <summary>
        /// 按钮预制路径
        /// </summary>
        public List<SelectListItem> ButtonPrefebPath { get; set; }

        /// <summary>
        /// 图片预制 路径
        /// </summary>
        public List<SelectListItem> ImgPrefebPath { get; set; }
        /// <summary>
        /// 视频 预制路径
        /// </summary>
        public List<SelectListItem> VideoPrefebPath { get; set; }

        public List<SelectListItem> AudioPrefebPath { get; set; }

        public List<SelectListItem> CameraPrefebPath { get; set; }

       


        public BookNodeNewRoot BookNodeRoot { get; set; }


       // public BookNodeNewRoot BookNodeNewRoot { get; set; }
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


        [NopResourceDisplayName("Admin.AiBook.BookNode.Fields.UniqueID")]
        public string UniqueID { get; set; }
    }
}
