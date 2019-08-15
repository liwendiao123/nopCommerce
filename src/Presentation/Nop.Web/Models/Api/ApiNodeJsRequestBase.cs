using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api
{
    public class ApiNodeJsRequestBase<T> where T:new()
    {
        public ApiNodeJsRequestBase()
        {
            data = new T();
        }

        public T data { get; set; }
    }
}
