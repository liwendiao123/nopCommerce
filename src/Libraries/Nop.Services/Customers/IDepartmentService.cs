using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core;
using Nop.Core.Domain.Customers;

namespace Nop.Services.Customers
{
  public  interface IDepartmentService
    {
        bool DeleteDep(Department dep);
        bool UpdateDep(Department dep);
        bool InsertDep(Department dep);
        Department GetDepById(int id);
        List<Department> GetAllDeps();
        IPagedList<Department> GetAllDeps(string name = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
    }
}
