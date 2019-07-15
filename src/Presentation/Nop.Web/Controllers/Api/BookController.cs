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
                data = product.Select(x => new {

                  //  x.ProductPictures
                   Id = x.Id,
                   Name = x.Name?.Trim(),
                   TestName = "fdsfafsaf 测试" + x.Name?.Trim(),
                   Cateid =string.Join("," ,x.ProductCategories.Select(y=>y.Id)),
                   Imgurl = "http://arbookresouce.73data.cn/book/img/sy_img_02.png",
                   x.DisplayOrder,
                   VendorName = "人民教育出版社",
                   Desc ="",
                   Tag = x.ProductProductTagMappings.Select(t=>new {
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