using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api.ApiBookNode
{
    public class JsonRequestKnowledgeByImgName:WebApiModel.ApiBaseRequest
    {
        public string imgName { get; set; }
    }
}
