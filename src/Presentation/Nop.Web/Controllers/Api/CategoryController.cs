using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Catalog;

namespace Nop.Web.Controllers.Api
{
    public class CategoryController : Controller
    {

        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetData()
        {

          var result =   _categoryService.GetAllCategoriesByParentCategoryId(14);

            return Json(new
            {
                code = 0,
                msg = "成功",
                data = result.Select(x => new {
                    id = x.Id,
                    pid = x.ParentCategoryId,
                    name = x.Name

                }).ToList()

            });

            //return View();
        }
    }
}