using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.LoginInfo;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Media;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Models.Api.Book;

namespace Nop.Web.Controllers.Api
{
    //[ValidateToken]
    public class BookController : BasePublicController
    {

        private readonly IProductService _productService;
        private readonly IPictureService _pictureService;
        private readonly ICustomerService _customerService;
        private readonly ICustomerOrderCodeService _customerOrderCodeService;
        public BookController(IProductService productService
                                ,IPictureService pictureService
                                ,ICustomerService customerService
                                ,ICustomerOrderCodeService customerOrderCodeService
                                )
        {
            _productService = productService;
            _pictureService = pictureService;
            _customerService = customerService;
            _customerOrderCodeService = customerOrderCodeService;
        }
        public IActionResult Index()
        {
            return View();
        }      
        public IActionResult GetBook(SearchBookRequest requst, string token, string qs_clientid)
        {
            var tokenresult =  ValidateToken(token, qs_clientid, _customerService);

            if (tokenresult == 1)
            {
                return Json(new
                {
                    code = -2,
                    msg = "该账号已在另一个地点登录,如不是本人操作，您的密码已经泄露,请及时修改密码！",
                    data = false

                });
            }
            if (tokenresult == 3)
            {
                return Json(new
                {
                    code = -3,
                    msg = "该账号已被禁用！",
                    data = false

                });
            }

            Customer customer = null;

            var isbuyList = new List<int>();
            var apitetoken = new AccountToken();
            apitetoken = AccountToken.Deserialize(token);
            var islogin = false;
            if (apitetoken != null)
            {
                //  

                int cid = 0;

                Int32.TryParse(apitetoken.ID, out cid);

                var bcustomer = _customerService.GetCustomerById(cid);

                if (bcustomer != null)
                {
                    customer = bcustomer;
                    var roles = bcustomer.CustomerCustomerRoleMappings.Select(x => x.CustomerRole).ToList();
                    if (roles.Where(x => !x.Name.Equals("Registered")).ToList().Exists(x => x.IsSystemRole))
                    {
                        islogin = true;
                    }
                    else
                    {
                        islogin = false;
                    }
                }

                islogin = true;
            }

            if (customer != null)
                isbuyList = customer.CustomerBooks.Select(x => x.ProductId).ToList();

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
                            var isbuy = isbuyList.Contains(x.Id);
                            var vendor = x.ProductManufacturers.FirstOrDefault();
                            string vendorname = "xxxxxxxxxxx";
                            if (vendor != null)
                            {
                                if (vendor.Manufacturer != null)
                                {
                                    vendorname = vendor.Manufacturer.Name;
                                }
                            }
                            var imgurl = string.Empty;
                            var defaultProductPicture = _pictureService.GetPicturesByProductId(x.Id, 1).FirstOrDefault();
                            if (defaultProductPicture != null)
                            {
                                imgurl = _pictureService.GetPictureUrl(defaultProductPicture);
                            }
                            if (!islogin && customer != null)
                            {
                                if (customer.CustomerBooks.Where(c => c.ProductId == x.Id).Count() > 0)
                                {
                                    islogin = true;
                                }
                            }
                            return new
                            {
                                Id = x.Id,
                                Name = x.Name?.Trim(),
                                TestName = x.Name?.Trim(),
                                Cateid = string.Join(",", x.ProductCategories.Select(y => y.Id)),
                                Imgurl = string.IsNullOrEmpty(imgurl) ? "http://arbookresouce.73data.cn/book/img/sy_img_02.png" : imgurl,
                                x.DisplayOrder,
                                IsBuy = isbuy,
                                VendorName = vendorname,
                                Desc = x.ShortDescription ?? "",
                                IsLock =(!islogin || !(0.Equals(x.Price))),
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
                            var isbuy = isbuyList.Contains(x.Id);
                            var imgurl = string.Empty;
                            var defaultProductPicture = _pictureService.GetPicturesByProductId(x.Id, 1).FirstOrDefault();

                            if (defaultProductPicture != null)
                            {
                                imgurl = _pictureService.GetPictureUrl(defaultProductPicture);
                            }
                            var vendor = x.ProductManufacturers.FirstOrDefault();

                            string vendorname = "xxxxxxxxxxx";
         
                            if (vendor != null)
                            {
                                if (vendor.Manufacturer != null)
                                {
                                    vendorname = vendor.Manufacturer.Name;
                                }
                            }
                            return new
                            {
                                Id = x.Id,
                                Name = x.Name?.Trim(),
                                TestName = x.Name?.Trim(),
                                Cateid = string.Join(",", x.ProductCategories.Select(y => y.Id)),
                                Imgurl = string.IsNullOrEmpty(imgurl) ? "http://arbookresouce.73data.cn/book/img/sy_img_02.png" : imgurl,
                                x.DisplayOrder,
                                IsBuy = isbuy,
                                VendorName = vendorname,
                                Desc = x.ShortDescription ?? "",
                                IsLock = !islogin,
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

                    var isbuy = isbuyList.Contains(x.Id);
                  var imgurl = string.Empty;
                    var defaultProductPicture = _pictureService.GetPicturesByProductId(x.Id, 1).FirstOrDefault();

                    if (defaultProductPicture != null)
                    {
                        imgurl = _pictureService.GetPictureUrl(defaultProductPicture);
                    }
                    var vendor = x.ProductManufacturers.FirstOrDefault();

                    string vendorname = "xxxxxxxxxxx";

                    if (vendor != null)
                    {
                        if (vendor.Manufacturer != null)
                        {
                            vendorname = vendor.Manufacturer.Name;
                        }
                        
                    }
                    return new
                    {
                        Id = x.Id,
                        Name = x.Name?.Trim(),
                        TestName = x.Name?.Trim(),
                        Cateid = string.Join(",", x.ProductCategories.Select(y => y.Id)),
                        Imgurl =string.IsNullOrEmpty(imgurl)? "http://arbookresouce.73data.cn/book/img/sy_img_02.png":imgurl,
                        x.DisplayOrder,
                        VendorName = vendorname,
                        Desc = x.ShortDescription??"",
                        IsLock = !islogin,
                        IsBuy = isbuy,
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="qs_clientid"></param>
        /// <param name="pid"></param>
        /// <param name="activecode"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public IActionResult Activate(string token, string qs_clientid, int pid,string activecode)
        {
            var tokenresult = ValidateToken(token, qs_clientid, _customerService);
            if (tokenresult == 1)
            {
                return Json(new
                {
                    code = -2,
                    msg = "该账号已在另一个地点登录,如不是本人操作，您的密码已经泄露,请及时修改密码！",
                    data = false
                });
            }
            if (tokenresult == 3)
            {
                return Json(new
                {
                    code = -3,
                    msg = "该账号已被禁用！",
                    data = false

                });
            }


            Customer customer = null;

            var isbuyList = new List<int>();
            var apitetoken = new AccountToken();
            apitetoken = AccountToken.Deserialize(token);
            var islogin = false;
            if (apitetoken != null)
            {
                int cid = 0;

                Int32.TryParse(apitetoken.ID, out cid);

                var bcustomer = _customerService.GetCustomerById(cid);

                if (bcustomer != null)
                {
                    customer = bcustomer;
                    var roles = bcustomer.CustomerCustomerRoleMappings.Select(x => x.CustomerRole).ToList();
                    if (roles.Where(x => !x.Name.Equals("Registered")).ToList().Exists(x => x.IsSystemRole))
                    {
                        islogin = true;
                    }
                    else
                    {
                        islogin = false;
                    }
                }
                islogin = true;
            }
            if (customer == null)
            {
                return Json(new
                {
                    code = -3,
                    msg = "该账号已被禁用！",
                    data = false

                });
            }

           
            var exitecode = _customerOrderCodeService.GetOrderCodeByCode(activecode);
            if (exitecode != null  )
            {
                var codeExpireDate = DateTime.Now;
                if (exitecode != null
                    && exitecode.CodeType == 0
                    && !exitecode.IsActived
                    && exitecode.IsValid
                    && exitecode.ProductId == pid
                    && exitecode.ProductId > 0
                     )
                {
                    var cb = customer.CustomerBooks.FirstOrDefault(x => x.ProductId == exitecode.ProductId);
                    if (string.IsNullOrEmpty(exitecode.Phone))
                    {
                        ActivatedBook(customer, exitecode, cb);
                        return Json(new
                        {
                            code = 0,
                            msg = "兑换码成功",
                            data = new
                            {
                            }
                        });
                    }
                    else
                    {
                        if (exitecode.Phone != customer.Username)
                        {
                            return Json(new
                            {
                                code = -1,
                                msg = "兑换码无效",
                                data = new
                                {
                                }
                            });
                        }
                        else
                        {
                            ActivatedBook(customer, exitecode, cb);
                            return Json(new
                            {
                                code = 0,
                                msg = "兑换码成功",
                                data = new
                                {
                                }
                            });
                        }
                    }

                }
                else
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "激活码无效",
                        data = new { }
                    });
                }


            }
            else
            {
                return Json(new
                {
                    code = -1,
                    msg = "激活码无效",
                    data = new { }
                });
            }


           // return View();
        }

        private void ActivatedBook(Customer customer, CustomerOrderCode exitecode, CustomerBook cb)
        {
            if (cb != null)
            {
                cb.UpdateTime = DateTime.Now;
                if (cb.Expirationtime < DateTime.Now)
                {
                    cb.Expirationtime = DateTime.Now.AddDays(exitecode.ValidDays);
                }
                else
                {
                    cb.Expirationtime = cb.Expirationtime.AddDays(exitecode.ValidDays);
                }




            }
            else
            {
                customer.CustomerBooks.Add(new CustomerBook
                {
                    CustomerId = customer.Id,
                    ProductId = exitecode.ProductId,
                    Status = 1,
                    CreateTime = DateTime.Now,
                    Expirationtime = DateTime.Now.AddDays(exitecode.ValidDays),
                    BookNodeId = 0,
                    BookBookDirId = 0,
                    TypeLabel = 1,
                    UpdateTime = DateTime.Now
                });
            }
            _customerService.UpdateCustomer(customer);
            exitecode.IsActived = true;
            _customerOrderCodeService.UpdateCustomerOrderCode(exitecode);
        }
    }
}