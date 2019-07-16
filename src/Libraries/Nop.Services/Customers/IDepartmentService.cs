using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core;
using Nop.Core.Domain.Customers;

namespace Nop.Services.Customers
{
  public  interface IDepartmentService
    {

        /// <summary>
        /// 删除部门信息
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        bool DeleteDep(Department dep);

        /// <summary>
        /// 更新部门信息
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        bool UpdateDep(Department dep);

        /// <summary>
        /// 插入部门信息
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        bool InsertDep(Department dep);

        /// <summary>
        ///  根据ID 获取部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Department GetDepById(int id);
        /// <summary>
        /// 获取所有学校
        /// </summary>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        List<Department> GetAllDeps(bool showHidden = false);
        /// <summary>
        /// 分页获取学校信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        IPagedList<Department> GetAllDeps(string name = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
    }
}
