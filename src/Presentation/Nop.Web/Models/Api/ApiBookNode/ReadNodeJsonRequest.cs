using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api.ApiBookNode
{
    public class ReadNodeJsonRequest:WebApiModel.ApiBaseRequest
    {
           public string guid { get; set; }

        public int pid { get; set; }
        public int mapperid { get; set; }
    }
}
