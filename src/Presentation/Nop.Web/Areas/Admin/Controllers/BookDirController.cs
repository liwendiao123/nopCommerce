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
using Nop.Services.Media;

namespace Nop.Web.Areas.Admin.Controllers
{
    public class BookDirController : BaseAdminController
    {
       
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;     
        private readonly IUrlRecordService _urlRecordService;
        private readonly IBookDirService _bookDirService;
        private readonly IBookDirFactory _bookDirFactory;
        private readonly IProductModelFactory _productModelFactory;
        private readonly IPictureService _pictureService;

        public BookDirController(
            IUrlRecordService urlRecordService
            ,IPermissionService permissionService
            ,ICustomerActivityService customerActivityService
            ,IEventPublisher eventPublisher
            ,ILocalizedEntityService localizedEntityService
            , ILocalizationService localizationService
            ,INotificationService notificationService
            ,IBookDirService bookDirService
            ,IBookDirFactory bookDirFactory
            ,IProductModelFactory productModelFactory
            ,IPictureService pictureService
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
            _localizedEntityService = localizedEntityService;
            _pictureService = pictureService;

        }
        public IActionResult Index()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagerBook))
                return AccessDeniedView();
            //prepare model
            var model = _bookDirFactory.PrepareBookDirSearchModel(new BookDirSearchModel(),new BookDirModel());

            return View(model);
        }


        #region List

        //public virtual IActionResult Index()
        //{
        //    return RedirectToAction("List");
        //}

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();

            //prepare model
            var model = _bookDirFactory.PrepareBookDirSearchModel(new BookDirSearchModel(),new BookDirModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(BookDirSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedDataTablesJson();    
            //prepare model
            var model = _bookDirFactory.PrepareBookDirSearchModel(searchModel,new BookDirModel());
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
            var model = _bookDirFactory.PrepareBookDirModel();
            return View(model);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Create(BookDirModel model, bool continueEditing)
        {  if (!_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return AccessDeniedView();

          

            if (ModelState.IsValid)
            {
                var category = model.ToEntity<BookDir>();
                category.CreatedOnUtc = DateTime.UtcNow;
                category.UpdatedOnUtc = DateTime.UtcNow;
                if (string.IsNullOrEmpty(category.PriceRanges))
                {
                    category.PriceRanges = "1";
                }
               

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(category, model.SeName, category.Name, true);
                _urlRecordService.SaveSlug(category, model.SeName, 0);

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

                _bookDirService.InsertBookDir(category);

               // _categoryService.UpdateCategory(category);

                //update picture seo file name
              //  UpdatePictureSeoNames(category);

                //ACL (customer roles)
               // SaveCategoryAcl(category, model);

                //stores
               // SaveStoreMappings(category, model);

//activity log
            _customerActivityService.InsertActivity("AddNewBookDir",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewBookDir"), category.Name), category);

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.AiBookDir.BookDir.AddNewBookDir"));

                if (!continueEditing)
                    return RedirectToAction("Index");
                
                return RedirectToAction("Edit", new { id = category.Id });
            }

            //prepare model
            model = _bookDirFactory.PrepareBookDirModel();

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            //try to get a category with the specified id
            //  var bookdir = _categoryService.GetCategoryById(id);
            var bookdir = _bookDirService.GetBookDirById(id);
            if (bookdir == null || bookdir.Deleted)
                return RedirectToAction("List");
            //prepare model
            //var model = _categoryModelFactory.PrepareCategoryModel(null, category);
            var model = _bookDirFactory.PrepareBookDirModel(null,bookdir);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Edit(BookDirModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagerBook))
                return AccessDeniedView();

            //try to get a category with the specified id
            var category = _bookDirService.GetBookDirById(model.Id);
            if (category == null || category.Deleted)
                return RedirectToAction("Index");
            model.CreatedOnUtc = category.CreatedOnUtc; 
            if (ModelState.IsValid)
            {
                var prevPictureId = category.PictureId;
                category = model.ToEntity(category);
                category.UpdatedOnUtc = DateTime.UtcNow;
                //.UpdateCategory(category);
                _bookDirService.UpdateBookDir(category);
                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(category, model.SeName, category.Name, true);
                _urlRecordService.SaveSlug(category, model.SeName, 0);
                //locales
                UpdateLocales(category, model);
                //delete an old picture (if deleted or updated)
                if (prevPictureId > 0 && prevPictureId != category.PictureId)
                {
                    var prevPicture = _pictureService.GetPictureById(prevPictureId);
                    if (prevPicture != null)
                        _pictureService.DeletePicture(prevPicture);
                }
                //update picture seo file name
               // UpdatePictureSeoNames(category);
                //ACL
                //SaveCategoryAcl(category, model);
                //stores
               // SaveStoreMappings(category, model);
                //activity log
                _customerActivityService.InsertActivity("EditBookDir",
                    string.Format(_localizationService.GetResource("ActivityLog.EditBookDir"), category.Name), category);
                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.AibookDir.EditBookDir.Updated"));
                if (!continueEditing)
                    return RedirectToAction("Index");
                return RedirectToAction("Edit", new { id = category.Id });
            }
            //prepare model
             model = _bookDirFactory.PrepareBookDirModel(model, category);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            if (!_permissionService.Authorize(StandardPermissionProvider.BookDirManage))
                return AccessDeniedView();
            var bookNode = _bookDirService.GetBookDirById(id);
            if (bookNode == null)
                return RedirectToAction("Index");
            _bookDirService.DeleteBookDir(bookNode);
            return RedirectToAction("Index");
         //   return View();
        }

        public IActionResult GetList(BookDirSearchModel searchModel)
        {
            //prepare model
            var model = _bookDirFactory.PrepareBookDirListModel(searchModel);
            return Json(model);
        }
        protected virtual void UpdateLocales(BookDir category, BookDirModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(category,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(category,
                    x => x.Description,
                    localized.Description,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(category,
                    x => x.MetaKeywords,
                    localized.MetaKeywords,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(category,
                    x => x.MetaDescription,
                    localized.MetaDescription,
                    localized.LanguageId);

                //_localizedEntityService.SaveLocalizedValue(category,
                //    x => x.MetaTitle,
                //    localized.MetaTitle,
                //    localized.LanguageId);

                //search engine name
                //var seName = _urlRecordService.ValidateSeName(category, localized.SeName, localized.Name, false);
                //_urlRecordService.SaveSlug(category, seName, localized.LanguageId);
            }
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

            //searchModel.PageSize = int.MaxValue;

            searchModel.SetGridPageSize(Int32.MaxValue);

            var model = _bookDirFactory.PrepareBookDirListModel(searchModel);
            return Json(model.Data.ToList());
        }
        #endregion
    }
}