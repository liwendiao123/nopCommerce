using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api.Ali
{
    public class RequestValidCode
    {
        public string Phone { get; set; }

        /// <summary>
        /// 注册类型
        /// </summary>
        public int Type { get; set; }


       public string token {get; set;
        }
        public string qs_clientid { get; set; }


    }
}
