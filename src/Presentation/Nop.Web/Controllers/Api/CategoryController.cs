using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Services.Catalog;
using Nop.Web.Models.Api.WebApiModel.ApiCategory;

namespace Nop.Web.Controllers.Api
{
    public class CategoryController : BasePublicController
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

        public IActionResult GetData(int id,string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    var jsonrequest = JsonConvert.DeserializeObject<JsonRequestGetData>(data);
                    if (jsonrequest != null)
                    {
                        id = jsonrequest.id;
                        //orderId = jsonrequest.orderId;
                        //inviteCode = jsonrequest.inviteCode;
                        //token = jsonrequest.token;
                        //qs_clientId = jsonrequest.qs_clientId;
                    }
                }
                catch (Exception ex)
                {

                }

            }


            var result =   _categoryService.GetAllCategoriesByParentCategoryId(14);
            return Json(new
            {
                code = 0,
                msg = "成功",
                data = result.Select(x => new {
                    id = x.Id,
                    pid = x.ParentCategoryId,
                    name = x.Name,
                    actualName = x.Name,
                    x.DisplayOrder
                }).ToList()

            });

            //return View();
        }
    }
}