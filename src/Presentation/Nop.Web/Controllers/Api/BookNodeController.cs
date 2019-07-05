using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.AIBookModel;
using Nop.Web.Framework.Controllers;

namespace Nop.Web.Controllers.Api
{
    public class BookNodeController : BaseController
    {
        private readonly IAiBookService _aiBookService;
        public BookNodeController(IAiBookService aiBookService)
        {
            _aiBookService = aiBookService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetData(int id)
        {

            var result = _aiBookService.GetAiBookModelById(id);

            return Json(new
            {
                code = 0,
                msg = "已成功",
                data = new
                {
                    strJson = result.StrJson
                }
            });
        }



    }
}