using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Models.Api.WebApiModel.AibookCommentModel
{
    public class JsonRequestAddBookCommentRequest:ApiBaseRequest
    {
        [NopResourceDisplayName("BookNode.Comments.CommentTitle")]
        public string CommentTitle { get; set; }
        [NopResourceDisplayName("BookNode.Comments.CommentText")]
        public string CommentText { get; set; }
        [NopResourceDisplayName("BookNode.Comments.BookNodeID")]
        public int BookNodeID { get; set; }
        [NopResourceDisplayName("BookNode.Comments.CustomerId")]
        public int CustomerId { get; set; }
        [NopResourceDisplayName("BookNode.Comments.DisplayCaptcha")]
        public bool DisplayCaptcha { get; set; }
    }
}
