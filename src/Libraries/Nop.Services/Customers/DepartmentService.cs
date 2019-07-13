using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core;
using Nop.Core.Domain.Customers;

namespace Nop.Services.Customers
{
    public class DepartmentService : BaseService, IDepartmentService
    {
        public bool DeleteDep(Department dep)
        {
            throw new NotImplementedException();
        }

        public List<Department> GetAllDeps()
        {
            throw new NotImplementedException();
        }

        public IPagedList<Department> GetAllDeps(string dep = null, string zipPostalCode = null, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            throw new NotImplementedException();
        }

        public Department GetDepById(int id)
        {
            throw new NotImplementedException();
        }

        public bool InsertDep(Department dep)
        {
            throw new NotImplementedException();
        }

        public bool UpdateDep(Department dep)
        {
            throw new NotImplementedException();
        }
    }
}
