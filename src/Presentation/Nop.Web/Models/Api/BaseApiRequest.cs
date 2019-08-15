using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api
{
    public class BaseApiRequest
    {
        public BaseApiRequest()
        {
            Token = string.Empty;

            qs_clientid = string.Empty;
        }
        public string Token { get; set; }

        public string qs_clientid { get;set; }
    }
}
