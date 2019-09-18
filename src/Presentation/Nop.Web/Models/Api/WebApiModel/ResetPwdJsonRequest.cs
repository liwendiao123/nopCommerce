using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api.WebApiModel
{
    public class ResetPwdJsonRequest:ApiBaseRequest
    {
       public string password { get; set; }
       public string userName { get; set; }
       public string code { get; set; }
    }
}
