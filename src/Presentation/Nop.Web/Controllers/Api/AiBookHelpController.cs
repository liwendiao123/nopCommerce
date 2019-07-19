using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Nop.Web.Controllers.Api
{
    public class AiBookHelpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetData()
        {
            return Json(new
            {
                code = 0,
                msg = "获取成功",
                //data = new List<>

            });
        }
    }

    public class HelpModelItem
    {
        public int Id { get; set; }


        public string Title { get; set; }


        public string Content { get; set; }

        public int Sort { get; set; }

    }
}