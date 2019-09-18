using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api.ApiBookNode
{
    public class JsonRequestBookNodeKeyName:WebApiModel.ApiBaseRequest
    {
        public string keyname { get; set; }
    }
}
