using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Catalog;
using Nop.Web.Models.Api.Book;

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
        public IActionResult GetBook(SearchBookRequest requst)
        {
            if (requst.CateId < 0)
            {
                requst.CateId = 14;
            }

            if (requst.Pageindex < 0)
            {
                requst.Pageindex = 0;

            }

            if (requst.PageSize < 4)
            {
                requst.PageSize = 4;
            }

           var product = _productService.SearchProducts(requst.Pageindex, requst.PageSize, new List<int>() { requst.CateId });
          
            return Json(new
            {
                code = 0,
                msg = "获取成功",
                data = product.Select(x => new {               
                   Id = x.Id,
                   Name = x.Name?.Trim(),
                   TestName =   x.Name?.Trim(),
                   Cateid =string.Join("," ,x.ProductCategories.Select(y=>y.Id)),
                   Imgurl = "http://arbookresouce.73data.cn/book/img/sy_img_02.png",
                   x.DisplayOrder,
                   VendorName = "广西人民出版社",
                   Desc =x.FullDescription,
                   x.Price,
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