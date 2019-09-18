using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Models.Api.WebApiModel;

namespace Nop.Web.Models.Api.ApiBookNode
{
    public class GetJsonDataJsonRequest:ApiBaseRequest
    {
        public int id { get; set; }
        public string platformtype { get; set; }
    }
}
