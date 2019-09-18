using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api.WebApiModel
{
    public class LoginRequest:ApiBaseRequest
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
}
