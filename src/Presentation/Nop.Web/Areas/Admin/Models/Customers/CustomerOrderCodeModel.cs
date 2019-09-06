using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Areas.Admin.Models.Common;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Customers
{
    public class CustomerOrderCodeModel : BaseNopModel
    {
        public CustomerOrderCodeModel()
        {
            OrderCodeModel = new OrderCodeModel();
        }
        public int CustomerId { get; set; }
        public OrderCodeModel OrderCodeModel { get; set; }
    }
}
