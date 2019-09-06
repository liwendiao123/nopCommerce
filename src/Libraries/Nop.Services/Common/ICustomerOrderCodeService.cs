using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.Customers;

namespace Nop.Services.Common
{
   public  interface ICustomerOrderCodeService
    {
        void DeleteCustomerOrderCode(CustomerOrderCode code);
        void UpdateCustomerOrderCode(CustomerOrderCode code);

        CustomerOrderCode GetOrderCodeByCode(string code);


    }
}
