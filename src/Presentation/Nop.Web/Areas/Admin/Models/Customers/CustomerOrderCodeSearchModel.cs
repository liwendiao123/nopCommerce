using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Customers
{
    public class CustomerOrderCodeSearchModel: BaseSearchModel
    {
         public int CustomerId { get; set; }

        public string Code { get; set; }
    }
}
