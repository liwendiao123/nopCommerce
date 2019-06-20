using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Nop.Web.Areas.Admin.Controllers
{
    public class BookDirController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}