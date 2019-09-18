using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api.WebApiModel
{
    public class CheckTokenRequest
    {
       public string token { get; set; }

        public string qs_clientid { get; set; }
    }
}
