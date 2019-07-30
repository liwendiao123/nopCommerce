using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.TableOfContent;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.TableOfContent;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions; 
using Nop.Web.Framework.Mvc.Filters;
using Nop.Core.Domain.TableOfContent;
using Nop.Services.AIBookModel;
using Nop.Web.Areas.Admin.Models.AiBook;
using Nop.Core.Domain.AIBookModel;
using Newtonsoft.Json;
using Nop.Web.Models.Api.BookNode;
using Microsoft.AspNetCore.Http;
using Nop.Services.ExportImport;

namespace Nop.Web.Areas.Admin.Controllers
{
    public class BookNodeController : BaseAdminController
    {
       
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;     
        private readonly IUrlRecordService _urlRecordService;
        private readonly IBookDirService _bookDirService;
        private readonly IBookDirFactory _bookDirFactory;
        private readonly IProductModelFactory _productModelFactory;
        private readonly IBookNodeFactory _bookNodeFactory;
        private readonly IAiBookService _bookNodeService;
        private readonly IImportManager _importManager;

        public BookNodeController(
            IUrlRecordService urlRecordService
            ,IPermissionService permissionService
            ,ICustomerActivityService customerActivityService
            ,IEventPublisher eventPublisher
            ,ILocalizationService localizationService
            ,INotificationService notificationService
            ,IBookDirService bookDirService
            ,IBookDirFactory bookDirFactory
            ,IProductModelFactory productModelFactory
            ,IBookNodeFactory bookNodeFactory
            ,IAiBookService bookNodeService,
            IImportManager importManager
            )
        {
            _urlRecordService = urlRecordService;
            _permissionService = permissionService;
            _customerActivityService = customerActivityService;
            _eventPublisher = eventPublisher;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _bookDirService = bookDirService;
            _bookDirFactory = bookDirFactory;
            _productModelFactory = productModelFactory;
            _bookNodeFactory = bookNodeFactory;
            _bookNodeService = bookNodeService;
            _importManager = importManager;

        }
        public IActionResult Index(AiBookSearchModelView smodel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagerBook))
                return AccessDeniedView();
            //prepare model
            //  var model = _bookDirFactory.PrepareBookDirSearchModel(new BookDirSearchModel(),new BookDirModel());
            var model = _bookNodeFactory.PrepareBookNodeSearchModel(smodel);
            return View(model);
        }
        #region List
        //public virtual IActionResult Index()
        //{
        //    return RedirectToAction("List");
        //}
        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.BookModelManage))
                return AccessDeniedView();
                var smodel = new AiBookSearchModelView();           
           // prepare model
          //  var model = _bookDirFactory.PrepareBookDirSearchModel(new BookDirSearchModel(),new BookDirModel());
            var model = _bookNodeFactory.PrepareBookNodeSearchModel(smodel);
            return View(model);
        }
        [HttpPost]
        public virtual IActionResult List(AiBookSearchModelView searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedDataTablesJson();

            //prepare model
            // var model = _bookDirFactory.PrepareBookDirSearchModel(searchModel,new BookDirModel());
            var model = _bookNodeFactory.PrepareBookNodeSearchModel(searchModel);
            return Json(model);
        }
        #endregion
        #region Create / Edit / Delete
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.BookDirManage))
                return AccessDeniedView();
            //prepare model
            //var model = _categoryModelFactory.PrepareCategoryModel(new CategoryModel(), null);
            var model = _bookNodeFactory.PrepareBookNodeModel(new AiBookModelView(),0);
            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Create(AiBookModelView  model, bool continueEditing)
        {  if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();
            if (ModelState.IsValid)
            {

                if (!string.IsNullOrEmpty(model.UnityStrJson))
                {

                    try
                    {
                        model.BookNodeRoot = JsonConvert.DeserializeObject<BookNodeNewRoot>(model.UnityStrJson);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }


                  
                }

                var category = model.ToEntity<AiBookModel>();
                category.CreatedOnUtc = DateTime.UtcNow;
                category.UpdatedOnUtc = DateTime.UtcNow;
                category.Published = true;
                //if (string.IsNullOrEmpty(category.PriceRanges))
                //{
                //    category.PriceRanges = "0";
                //}


                //search engine name
                //  model.SeName = _urlRecordService.ValidateSeName(category, model.SeName, category.Name, true);
                // _urlRecordService.SaveSlug(category, model.SeName, 0);

                //locales
                // UpdateLocales(category, model);

                //discounts
                //    var allDiscounts = _discountService.GetAllDiscounts(DiscountType.AssignedToCategories, showHidden: true);
                //            foreach (var discount in allDiscounts)
                //            {
                //                if (model.SelectedDiscountIds != null && model.SelectedDiscountIds.Contains(discount.Id))
                //                    //category.AppliedDiscounts.Add(discount);
                //                    category.DiscountCategoryMappings.Add(new DiscountCategoryMapping { Discount = discount
                //});
                //            }

                // _bookDirService.InsertBookDir(category);
                _bookNodeService.InsertAiBookModel(category);
               // _categoryService.UpdateCategory(category);

                //update picture seo file name
              //  UpdatePictureSeoNames(category);

                //ACL (customer roles)
               // SaveCategoryAcl(category, model);

                //stores
               // SaveStoreMappings(category, model);

//activity log
            _customerActivityService.InsertActivity("AddNewBookNode",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewBookNode"), category.Name), category);

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.AiBookDir.BookNode.AddNewBookNode"));

                if (!continueEditing)
                    return RedirectToAction("Index");
                
                return RedirectToAction("Edit", new { id = category.Id });
            }
            //prepare model
            model = _bookNodeFactory.PrepareBookNodeModel(new AiBookModelView(), 0);        
            //if we got this far, something failed, redisplay form
            return View(model);
        }
        public IActionResult Edit(int id)
        {
            //try to get a category with the specified id
            //  var bookdir = _categoryService.GetCategoryById(id);
            var booknode = _bookNodeService.GetAiBookModelById(id);
            if (booknode == null || booknode.Deleted)
                return RedirectToAction("List");
            //prepare model
            //var model = _categoryModelFactory.PrepareCategoryModel(null, category);
            // var model = _bookDirFactory.PrepareBookDirModel(null,bookdir);
            var aimodel = booknode.ToModel<AiBookModelView>();
            var model = _bookNodeFactory.PrepareBookNodeModel(aimodel, 0);
            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Edit(AiBookModelView model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagerBook))
            {
                return AccessDeniedView();
            }
            try
            {
                var result = _bookNodeService.GetAiBookModelById(model.Id);
                ///检验 该知识点是否存在
                if (result == null || result.Deleted)
                {
                    return RedirectToAction("Index");
                }    
                model.CreatedOnUtc = result.CreatedOnUtc;
                model.UpdatedOnUtc = DateTime.Now;
                result = model.ToEntity(result);
                _bookNodeService.UpdateAiBookModel(result);

                model = result.ToModel<AiBookModelView>();
                //_bookDirService.UpdateBookDir(category);
                //search engine name
                //result.SeName = _urlRecordService.ValidateSeName(category, model.SeName, category.Name, true);
                //_urlRecordService.SaveSlug(category, model.SeName, 0);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return RedirectToAction("Index");
            }
           var model1 = _bookNodeFactory.PrepareBookNodeModel(model, 0);
           return View(model1);

        }       
        public IActionResult Delete(int id)
        {

            if (!_permissionService.Authorize(StandardPermissionProvider.BookDirManage))
                return AccessDeniedView();

            //try to get a blog post with the specified id
            var bookNode = _bookNodeService.GetAiBookModelById(id);
            if (bookNode == null)
                return RedirectToAction("Index");

            _bookNodeService.DeleteAiBookModel(bookNode);
            //_blogService.DeleteBlogPost(bookNode);
            //activity log
            _customerActivityService.InsertActivity("DeleteBookNode",
                string.Format(_localizationService.GetResource("ActivityLog.DeleteBookNode"), bookNode.Id), bookNode);

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Aibook.BookManage.BookNode.Deleted"));

            return RedirectToAction("Index");
            //return View();
        }
        public IActionResult GetList(AiBookSearchModelView searchModel)
        {
            //prepare model
            var model = _bookNodeFactory.PrepareBookNodeListModel(searchModel);

               return Json(model);
        }
        /// <summary>
        /// 获取所属书籍
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public IActionResult GetBooksByCategory(BookDirSearchModel searchModel)
        {
           var result =  _productModelFactory.PrepareProductListModel(
                new Models.Catalog.ProductSearchModel()
                {
                     SearchCategoryId = searchModel.CategoryID
                });

            return Json(result.Data.ToList());
        }
        public IActionResult GetBookDirByBookId(BookDirSearchModel searchModel)
        {
           var model = _bookDirFactory.PrepareBookDirListModel(searchModel);

            return Json(model.Data.ToList());
        }

        [HttpPost]
        public IActionResult ImportExcel(IFormFile importexcelfile)
        {
            _importManager.ImportBookNodeMobanFromXlsx(importexcelfile.OpenReadStream());
            

            return View();
        }
        #endregion
    }
}