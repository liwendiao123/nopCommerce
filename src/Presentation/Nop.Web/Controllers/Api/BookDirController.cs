using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.TableOfContent;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.TableOfContent;
using Nop.Web.Models.AiBook;

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
                  BookDirId = bookdirId
            };
            var result =  _bookDirService.GetAllBookDirsData("",0,bookid, bookdirId).ToList();
            result.ForEach(x =>
            {
                x.BookNodeUrl = Request.Scheme + "://" + Request.Host + "BookNode/GetData?id=" + x.Id;
            });
            //  var model = _bookDirFactory.PrepareBookDirSearchModel(searchModel, new BookDirModel());
          var treeresult =result.ToList();
          var  list = new List<BookDirTreeModel>();
            treeresult.ToList().ForEach(x =>
            {
                list.Add(new BookDirTreeModel
                {
                    Id=  x.Id,  //章节ID
                    PId = x.ParentBookDirId, //上级ID
                    IsLock = true,///是否已经购买 解锁
                    BookID= x.BookID, //所属课本ID
                    Name=  x.Name, //章节名称
                    IsRead = false,
                    Description = x.Description,//章节描述
                    PriceRanges = x.PriceRanges??"0",//价格描述  如果为零 则免费 否则展示需要付费的价格
                    DisplayOrder =  x.DisplayOrder,//展示顺序
                    IsLastNode = x.IsLastNode,  //是否为知识点
                    ComplexLevel =  x.ComplexLevel, //收费费复杂知识点
                    ImgUrl = "http://arbookresouce.73data.cn/book/img/sy_img_02.png",//封面展示
                    BookNodeUrl =x.BookNodeUrl //获取对应知识点 Url 
                });

            });
            var resl = new List<BookDirTreeModel>();
           var  resl1 = SortBookDirsForTree(list, resl, new List<int>(), 0);
            resl = resl1.ToList();

            return Json(new {
                code = 0,
                msg = "获取成功",
                data = resl
                //data = treeresult.Select(x=>new
                //{
                //    x.Id,  //章节ID
                //    PId = x.ParentBookDirId, //上级ID
                //    IsLock = true ,///是否已经购买 解锁
                //    x.BookID, //所属课本ID
                //    x.Name, //章节名称
                //    x.Description,//章节描述
                //    x.PriceRanges,//价格描述  如果为零 则免费 否则展示需要付费的价格
                //    x.DisplayOrder,//展示顺序
                //    x.IsLastNode,  //是否为知识点
                //    x.ComplexLevel, //收费费复杂知识点
                //    ImgUrl = "http://arbookresouce.73data.cn/book/img/sy_img_02.png",//封面展示
                //    x.BookNodeUrl //获取对应知识点 Url 
                //}).ToList()
            });
        }



        public IActionResult GetBookdirClient(int bookid,int bookdirId)
        {
            BookDirSearchModel searchModel = new BookDirSearchModel
            {
                BookID = bookid,
                BookDirId = bookdirId
            };
            var result = _bookDirService.GetAllBookDirsData("", 0, bookid, bookdirId).ToList();
            result.ForEach(x =>
            {
                x.BookNodeUrl = Request.Scheme + "://" + Request.Host + "BookNode/GetData?id=" + x.Id;
            });
            //  var model = _bookDirFactory.PrepareBookDirSearchModel(searchModel, new BookDirModel());
            var treeresult = result.ToList();
            var list = new List<BookDirTreeModel>();
            treeresult.ToList().ForEach(x =>
            {
                list.Add(new BookDirTreeModel
                {
                    Id = x.Id,  //章节ID
                    PId = x.ParentBookDirId, //上级ID
                    IsLock = true,///是否已经购买 解锁
                    BookID = x.BookID, //所属课本ID
                    Name = x.Name, //章节名称
                    IsRead = false,
                    Description = x.Description,//章节描述
                    PriceRanges = x.PriceRanges ?? "0",//价格描述  如果为零 则免费 否则展示需要付费的价格
                    DisplayOrder = x.DisplayOrder,//展示顺序
                    IsLastNode = x.IsLastNode,  //是否为知识点
                    ComplexLevel = x.ComplexLevel, //收费费复杂知识点
                    ImgUrl = "http://arbookresouce.73data.cn/book/img/sy_img_02.png",//封面展示
                    //获取对应知识点 Url"
                    BookNodeUrl = "http://www.73data.cn/EduProject/Sports.php?id="+ x.Id
                                   
                });

            });
            var resl = new List<BookDirTreeModel>();
            var resl1 = SortBookDirsForTree(list, resl, new List<int>(), 0);
            resl = resl1.ToList();

            return Json(new
            {
                code = 0,
                msg = "获取成功",
                data = resl
                //data = treeresult.Select(x=>new
                //{
                //    x.Id,  //章节ID
                //    PId = x.ParentBookDirId, //上级ID
                //    IsLock = true ,///是否已经购买 解锁
                //    x.BookID, //所属课本ID
                //    x.Name, //章节名称
                //    x.Description,//章节描述
                //    x.PriceRanges,//价格描述  如果为零 则免费 否则展示需要付费的价格
                //    x.DisplayOrder,//展示顺序
                //    x.IsLastNode,  //是否为知识点
                //    x.ComplexLevel, //收费费复杂知识点
                //    ImgUrl = "http://arbookresouce.73data.cn/book/img/sy_img_02.png",//封面展示
                //    x.BookNodeUrl //获取对应知识点 Url 
                //}).ToList()
            });

        }

        /// <summary>
        /// Sort categories for tree representation
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="parentId">Parent category identifier</param>
        /// <param name="ignoreCategoriesWithoutExistingParent">A value indicating whether categories without parent category in provided category list (source) should be ignored</param>
        /// <returns>Sorted categories</returns>
        public virtual IList<BookDirTreeModel> SortBookDirsForTree(IList<BookDirTreeModel> source, List<BookDirTreeModel> list,List<int> ids ,int parentId = 0,
            bool ignoreCategoriesWithoutExistingParent = false)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            //var result = new List<BookDirTreeModel>();
            if (list == null)
            {
                list = new List<BookDirTreeModel>();
            }

            if (ids == null)
            {
                ids = new List<int>();
            }
            var result = source.FirstOrDefault(x => x.Id == parentId);
            var children = source.Where(c => c.PId == parentId).ToList();

            if (parentId == 0)
            {
                children = source.Where(c => c.PId <1).ToList();
            }
            if (result != null)
            {
               
                if (!ids.Contains(result.Id))
                {
                    list.Add(result);
                    ids.Add(result.Id);
                }

                if (children != null && children.Count > 0)
                {


                    // result.Children.AddRange(children);
                    children.Select(x => x).ToList().ForEach(x =>
                    {

                        if (!ids.Contains(x.Id))
                        {
                            
                            ids.Add(x.Id);
                        }

                        if (!result.Children.Exists(c => c.Id == x.Id))
                        {
                            result.Children.Add(x);
                        }

                    });


                }

            }
            else
            {
                //list.AddRange(children);
                children.Select(x => x).ToList().ForEach(x =>
                {
                    if (!ids.Contains(x.Id))
                    {
                        ids.Add(x.Id);
                    }
                    if (!list.Exists(c => c.Id == x.Id))
                    {
                        list.Add(x);
                    }
                });
            }
            foreach (var cat in children)
            {

                SortBookDirsForTree(source, list, ids, cat.Id, true);
            }

            //foreach (var cat in source.Where(c => c.PId == parentId).ToList())
            //{

            //   SortBookDirsForTree(source, list, cat.Id, true));
            //}

            //if (ignoreCategoriesWithoutExistingParent || result.Count == source.Count)
            //    return result;

            ////find categories without parent in provided category source and insert them into result
            //foreach (var cat in source)
            //    if (result.FirstOrDefault(x => x.Id == cat.Id) == null)
            //        result.Add(cat);

            return list;
        }
    }
}