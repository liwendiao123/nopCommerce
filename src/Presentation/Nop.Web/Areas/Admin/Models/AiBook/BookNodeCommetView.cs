using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.AiBook
{
    public class BookNodeCommetView : BaseNopEntityModel
    {
        #region Properties
        [NopResourceDisplayName("Admin.ContentManagement.BookNode.Comments.Fields.NewsItem")]
        public int BookNodeId { get; set; }
        [NopResourceDisplayName("Admin.ContentManagement.BookNode.Comments.Fields.NewsItem")]
        public string BookNodeName { get; set; }
        [NopResourceDisplayName("Admin.ContentManagement.BookNode.Comments.Fields.Customer")]
        public int CustomerId { get; set; }   
        [NopResourceDisplayName("Admin.ContentManagement.BookNode.Comments.Fields.Customer")]
        public string CustomerInfo { get; set; }
        [NopResourceDisplayName("Admin.ContentManagement.BookNode.Comments.Fields.CommentTitle")]
        public string CommentTitle { get; set; }  
        [NopResourceDisplayName("Admin.ContentManagement.BookNode.Comments.Fields.CommentText")]
        public string CommentText { get; set; }
        [NopResourceDisplayName("Admin.ContentManagement.BookNode.Comments.Fields.IsApproved")]
        public bool IsApproved { get; set; }
        [NopResourceDisplayName("Admin.ContentManagement.BookNode.Comments.Fields.StoreName")]
        public int StoreId { get; set; }  
        public string StoreName { get; set; }
        [NopResourceDisplayName("Admin.ContentManagement.BookNode.Comments.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion

    }
}
