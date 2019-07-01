using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.AiBook
{
    public partial class AiBookSearchModelView : BaseSearchModel
    {

        #region Field

        #endregion
        #region Cotor
        public AiBookSearchModelView()
        {
            AvailableCategories = new List<SelectListItem>();
            AvailableBooks = new List<SelectListItem>();
            AvailableBookDirs = new List<SelectListItem>();
        }
        #endregion
        [NopResourceDisplayName("Admin.AiBook.BookModel.List.SearchCategory")]
        public int CateId { get; set; }

        [NopResourceDisplayName("Admin.AiBook.BookModel.List.SearchBook")]
        public int BookId { get; set; }

        [NopResourceDisplayName("Admin.AiBook.BookModel.List.SearchBookDir")]
        public int BookDirId { get; set; }

        [NopResourceDisplayName("Admin.AiBook.BookModel.List.Name")]
        public string BookAiModelName { get; set; }


        [NopResourceDisplayName("Admin.AiBook.BookModel.List.BookType")]
        public int BookType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// 
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

    }
}
