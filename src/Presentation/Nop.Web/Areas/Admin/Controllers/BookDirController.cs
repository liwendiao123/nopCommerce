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

namespace Nop.Web.Areas.Admin.Controllers
{
    public class BookDirController : BaseAdminController
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

        public BookDirController(
            IUrlRecordService urlRecordService
            ,IPermissionService permissionService
            ,ICustomerActivityService customerActivityService
            ,IEventPublisher eventPublisher
            ,ILocalizationService localizationService
            ,INotificationService notificationService
            ,IBookDirService bookDirService
            ,IBookDirFactory bookDirFactory
            ,IProductModelFactory productModelFactory
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


        public IActionResult Edit()
        {

            if (!_permissionService.Authorize(StandardPermissionProvider.BookDirManage))
                return AccessDeniedView();
            return View();
            
        }

        public IActionResult Delete()
        {

            if (!_permissionService.Authorize(StandardPermissionProvider.BookDirManage))
                return AccessDeniedView();

            return View();
        }


        public IActionResult GetList(BookDirSearchModel searchModel)
        {


            //prepare model
            var model = _bookDirFactory.PrepareBookDirListModel(searchModel);

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

            return Json(model);
        }
        #endregion
    }
}