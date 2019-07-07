using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Catalog;

namespace Nop.Web.Controllers.Api
{
    public class BookController : Controller
    {

        private readonly IProductService _productService;

        public BookController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetBook(int cateid)
        {

            if (cateid < 0)
            {
                cateid = 14;
            }

           var product = _productService.SearchProducts(0, int.MaxValue, new List<int>() {cateid });


            return Json(new
            {
                code = 0,
                msg = "",
                Data = product.Select(x => new {
                   id = x.Id,
                   name = x.Name,
                   cateid =string.Join("," ,x.ProductCategories.Select(y=>y.Id)),
                   imgurl = "",
                   x.DisplayOrder,
                   tag = x.ProductProductTagMappings.Select(t=>new {
                        BookId = t.ProductId,
                        Tag = t.ProductTag,
                        TagId = t.ProductTagId
                   }).ToList()
                })

            });

        
            //return View();
        }

    }
}