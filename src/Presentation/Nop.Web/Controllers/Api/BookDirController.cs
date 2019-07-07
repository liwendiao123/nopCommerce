using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.TableOfContent;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.TableOfContent;

namespace Nop.Web.Controllers.Api
{
    public class BookDirController : Controller
    {
        private readonly IBookDirService _bookDirService;
        private readonly IBookDirFactory _bookDirFactory;
        private readonly IProductModelFactory _productModelFactory;
        public BookDirController(  
            IBookDirService bookDirService
            , IBookDirFactory bookDirFactory
            , IProductModelFactory productModelFactory)
        {
            _bookDirService = bookDirService;
            _bookDirFactory = bookDirFactory;
            _productModelFactory = productModelFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetBookDir(int bookid,int bookdirId)
        {
            BookDirSearchModel searchModel = new BookDirSearchModel
            {
                 BookID = bookid,
                  BookDirId = bookid
            };

          var result =  _bookDirService.GetAllBookDirsData("",0,bookid,bookid).ToList();

            result.ForEach(x =>
            {
                x.BookNodeUrl = Request.Scheme + "://" + Request.Host + "BookNode/GetData?id=" + x.Id;

            });
            //  var model = _bookDirFactory.PrepareBookDirSearchModel(searchModel, new BookDirModel());
          var treeresult =  _bookDirService.SortBookDirsForTree(result);
            return Json(new {
                code = 0,
                msg = "获取成功",
                Data = treeresult
            });
        }
    }
}