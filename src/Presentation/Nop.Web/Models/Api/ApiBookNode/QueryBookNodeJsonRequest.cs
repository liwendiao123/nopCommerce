using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Models.Api.WebApiModel;

namespace Nop.Web.Models.Api.ApiBookNode
{
    public class QueryBookNodeJsonRequest:ApiBaseRequest
    {
        public int Id { get; set; }
    }
}
