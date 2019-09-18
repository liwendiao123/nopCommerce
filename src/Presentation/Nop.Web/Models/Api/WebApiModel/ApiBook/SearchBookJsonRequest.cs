using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api.WebApiModel.ApiBook
{
    public class SearchBookJsonRequest:BaseApiRequest
    {
        public int CateId { get; set; }
        public int BookId { get; set; }
        public int Pageindex { get; set; }
        public int PageSize { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public string UseName { get; set; }
    }
}
