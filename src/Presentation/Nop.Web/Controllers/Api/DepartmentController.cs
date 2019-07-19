using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Customers;
using Nop.Web.Infrastructure;
using Nop.Web.Models.Customer;

namespace Nop.Web.Controllers.Api
{
    public class DepartmentController : Controller
    {

        private IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

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
                     Name = "广西南宁三中"
                },
                new DepartmentList{
                    Id = 2,
                    Name = "南宁二中"
                }
            };
           var deps =  _departmentService.GetAllDeps(true);
            if (deps == null || deps.Count == 0)
            {
                return Json(new
                {
                    code = 0,
                    msg = "获取成功",
                    data = list.Select(x => new
                    {
                        x.Id,
                        x.Name,
                        FirstChar = PingYinHelper.GetFirstSpell(x.Name)
                    })

                });
            }
            else
            {
                return Json(new
                {
                    code =0,
                    msg = "获取成功",
                    data = deps.Select(x=> new{

                        x.Id,
                        x.Name,
                        FirstChar = PingYinHelper.GetFirstSpell(x.Name)

                    }).ToList()
                     


                });
            }

            return Json(new
            {
                code = 0,
                msg = "获取成功",
                data = list.Select(x => new{

                    x.Id,
                    x.Name,
                    FirstChar = PingYinHelper.GetFirstSpell(x.Name)
                })

            });

        }
    }
}