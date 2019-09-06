using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Data;
using Nop.Core.Domain.Customers;
using System.Linq;

namespace Nop.Services.Common
{
    public class CustomerOrderCodeService : ICustomerOrderCodeService
    {
       private readonly IRepository<CustomerOrderCode> _customerOrderCodeRepository;

        public CustomerOrderCodeService(IRepository<CustomerOrderCode> customerOrderCodeRepository)
        {
            _customerOrderCodeRepository = customerOrderCodeRepository;
        }
        public void DeleteCustomerOrderCode(CustomerOrderCode code)
        {
            throw new NotImplementedException();
        }

        public CustomerOrderCode GetOrderCodeByCode(string code)
        {
            var query = _customerOrderCodeRepository.Table;
            return query.Where(x => x.OrderCode == code).FirstOrDefault();
        }

        public void UpdateCustomerOrderCode(CustomerOrderCode code)
        {
            _customerOrderCodeRepository.Update(code);
        }
    }
}
