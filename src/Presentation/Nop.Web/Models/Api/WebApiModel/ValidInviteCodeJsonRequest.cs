using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api.WebApiModel
{
    public class ValidInviteCodeJsonRequest:ApiBaseRequest
    {
        public string code { get; set; }
    }
}
