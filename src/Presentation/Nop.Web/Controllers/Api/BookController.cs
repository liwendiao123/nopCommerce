using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.Media;
using Nop.Web.Models.Api.Book;

namespace Nop.Web.Controllers.Api
{
    public class BookController : Controller
    {

        private readonly IProductService _productService;
        private readonly IPictureService _pictureService;
        public BookController(IProductService productService
                                ,IPictureService pictureService
                                )
        {
            _productService = productService;
            _pictureService = pictureService;
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

            if (requst.bookId > 0)
            {
              var result =  _productService.GetProductById(requst.bookId);


                List<Product> products = new List<Product>();

                if (result != null)
                {
                    products.Add(result);

                    return Json(new
                    {
                        code = 0,
                        msg = "获取数据成功",
                        data = products.Select(x => {

                            var imgurl = string.Empty;
                            var defaultProductPicture = _pictureService.GetPicturesByProductId(x.Id, 1).FirstOrDefault();

                            if (defaultProductPicture != null)
                            {
                                imgurl = _pictureService.GetPictureUrl(defaultProductPicture);
                            }

                            return new
                            {
                                Id = x.Id,
                                Name = x.Name?.Trim(),
                                TestName = x.Name?.Trim(),
                                Cateid = string.Join(",", x.ProductCategories.Select(y => y.Id)),
                                Imgurl = string.IsNullOrEmpty(imgurl) ? "http://arbookresouce.73data.cn/book/img/sy_img_02.png" : imgurl,
                                x.DisplayOrder,
                                VendorName = "广西人民出版社",
                                Desc = x.ShortDescription ?? "",
                                IsLock = false,
                                x.Price,
                                Tag = x.ProductProductTagMappings.Select(t => new
                                {
                                    BookId = t.ProductId,
                                    Tag = t.ProductTag,
                                    TagId = t.ProductTagId
                                }).ToList()
                            };

                        })
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "课本不存在",
                        data = products.Select(x => {

                            var imgurl = string.Empty;
                            var defaultProductPicture = _pictureService.GetPicturesByProductId(x.Id, 1).FirstOrDefault();

                            if (defaultProductPicture != null)
                            {
                                imgurl = _pictureService.GetPictureUrl(defaultProductPicture);
                            }

                            return new
                            {
                                Id = x.Id,
                                Name = x.Name?.Trim(),
                                TestName = x.Name?.Trim(),
                                Cateid = string.Join(",", x.ProductCategories.Select(y => y.Id)),
                                Imgurl = string.IsNullOrEmpty(imgurl) ? "http://arbookresouce.73data.cn/book/img/sy_img_02.png" : imgurl,
                                x.DisplayOrder,
                                VendorName = "广西人民出版社",
                                Desc = x.ShortDescription ?? "",
                                IsLock = false,
                                x.Price,
                                Tag = x.ProductProductTagMappings.Select(t => new
                                {
                                    BookId = t.ProductId,
                                    Tag = t.ProductTag,
                                    TagId = t.ProductTagId
                                }).ToList()
                            };

                        })
                    });
                }

            }
      

           var product = _productService.SearchProducts(requst.Pageindex, requst.PageSize, new List<int>() { requst.CateId });
            //product.Select(x =>
            //{
            //    var defaultProductPicture = _pictureService.GetPicturesByProductId(product.Id, 1).FirstOrDefault();
            //    x.Img = _pictureService.GetPictureUrl(defaultProductPicture, 75);
            //    return x;
            //});
       
            return Json(new
            {
                code = 0,
                msg = "获取成功",
                data = product.Select(x =>  {

                  var imgurl = string.Empty;
                    var defaultProductPicture = _pictureService.GetPicturesByProductId(x.Id, 1).FirstOrDefault();

                    if (defaultProductPicture != null)
                    {
                        imgurl = _pictureService.GetPictureUrl(defaultProductPicture);
                    }
                   
                    return new
                    {
                        Id = x.Id,
                        Name = x.Name?.Trim(),
                        TestName = x.Name?.Trim(),
                        Cateid = string.Join(",", x.ProductCategories.Select(y => y.Id)),
                        Imgurl =string.IsNullOrEmpty(imgurl)? "http://arbookresouce.73data.cn/book/img/sy_img_02.png":imgurl,
                        x.DisplayOrder,
                        VendorName = "广西人民出版社",
                        Desc = x.ShortDescription??"",
                        IsLock = false,
                        x.Price,
                        Tag = x.ProductProductTagMappings.Select(t => new
                        {
                            BookId = t.ProductId,
                            Tag = t.ProductTag,
                            TagId = t.ProductTagId
                        }).ToList()
                    };
                
                })
            });

        
            //return View();
        }

    }
}