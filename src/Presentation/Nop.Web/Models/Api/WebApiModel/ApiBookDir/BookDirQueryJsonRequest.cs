using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api.WebApiModel.ApiBookDir
{
    public class BookDirQueryJsonRequest:ApiBaseRequest
    {
       public int bookid { get; set; }
       public int bookdirId { get; set; }
       public string token { get; set; }
       public string qs_clientid { get; set; }
    }
}
