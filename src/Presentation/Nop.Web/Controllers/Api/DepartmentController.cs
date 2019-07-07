using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Models.Customer;

namespace Nop.Web.Controllers.Api
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult GetList()
        {

            List<DepartmentList> list = new List<DepartmentList>(
            )
            {
                  new DepartmentList{
                     Id = 1,
                     Name = "南宁三中"
                },
                new DepartmentList{
                    Id = 2,
                    Name = "南宁二中"
                }
            };

            return Json(new
            {
                code = 0,
                msg = "获取成功",
                data = list

            });

        }
    }
}