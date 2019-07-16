using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Web.Areas.Admin.Models.Departments;

namespace Nop.Web.Areas.Admin.Factories
{
   public interface IDepartmentFactory
    {
        
        /// <summary>
        /// prapare search model
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        DepartmentSearchModel PrepareDepSearchModel(DepartmentSearchModel searchModel);


        DepartmentListModel PrepareDepartmentListModel(DepartmentSearchModel searchModel);

        DepartmentModel PrepareDepModel(DepartmentModel model, Department department, bool excludeProperties = false);

    }
}
