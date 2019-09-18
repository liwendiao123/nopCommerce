using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api.WebApiModel
{
    public class SmsMsgRecordRequest
    {
       public string phone { get; set; }

       public string code { get; set; }
       public int type { get; set; }
    }
}
