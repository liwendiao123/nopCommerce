using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Departments
{
    public class DepartmentSearchModel: BaseSearchModel
    {

        [NopResourceDisplayName("Admin.Department.Fields.Keywords")]
        public string KeyWords { get; set; }
    }
}
