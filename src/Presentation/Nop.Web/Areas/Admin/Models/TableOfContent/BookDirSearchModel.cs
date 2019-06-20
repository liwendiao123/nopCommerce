using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.TableOfContent
{
    public class BookDirSearchModel: BaseSearchModel
    {
        #region Ctor  构造函数

        public BookDirSearchModel()
        {
            AvailableStores = new List<SelectListItem>();
            AvailableBooks = new List<SelectListItem>();
        }
        #endregion
        #region Properties
        [NopResourceDisplayName("Admin.AiBook.BookDir.List.SearchCategoryName")]
        public string SearchDirName { get; set; }
        [NopResourceDisplayName("Admin.AiBook.BookDir.List.SearchStore")]
        public int SearchStoreId { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [NopResourceDisplayName("Admin.AiBook.BookDir.List.AvailableBooks")]
        public IList<SelectListItem> AvailableBooks { get; set; }

        public bool HideStoresList { get; set; }
        #endregion
    }
}
