using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.AIBookModel;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Framework.Controllers;

namespace Nop.Web.Controllers.Api
{
    public class BookNodeController : BaseController
    {
        private readonly IAiBookService _aiBookService;
        private readonly IBookNodeFactory _bookNodeFactory;
        public BookNodeController(IAiBookService aiBookService, IBookNodeFactory bookNodeFactory)
        {
            _aiBookService = aiBookService;
            _bookNodeFactory = bookNodeFactory;
        }

        public IActionResult Index()
        {

            var model = _bookNodeFactory.PrepareBookNodeListModel(new Areas.Admin.Models.AiBook.AiBookSearchModelView());

            return View(model);
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