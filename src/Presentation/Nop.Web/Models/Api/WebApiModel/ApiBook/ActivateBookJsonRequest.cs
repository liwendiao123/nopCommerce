using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api.WebApiModel.ApiBook
{
    public class ActivateBookJsonRequest:ApiBaseRequest
    {
         public   int pid { get; set; }
        public string activecode { get; set; }
    }
}
