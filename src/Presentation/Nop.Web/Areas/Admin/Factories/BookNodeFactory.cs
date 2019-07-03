using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Security;
using Nop.Services.AIBookModel;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Seo;
using Nop.Services.TableOfContent;
using Nop.Web.Areas.Admin.Helpers;
using Nop.Web.Areas.Admin.Models.AiBook;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Core.Domain.AIBookModel;

namespace Nop.Web.Areas.Admin.Factories
{

    /// <summary>
    /// 
    /// </summary>
    public class BookNodeFactory : IBookNodeFactory
    {


        #region Field
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly CaptchaSettings _captchaSettings;
        private readonly CustomerSettings _customerSettings;
        private readonly IAiBookService _aiBookService;
        private readonly ICategoryModelFactory _categoryModelFactory;
        private readonly ICustomerService _customerService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IPictureService _pictureService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IStoreContext _storeContext;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;
        private readonly MediaSettings _mediaSettings;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IProductService _productService;
        private readonly IBookDirFactory _bookDirFactory;
        private readonly IBookDirService _bookdirService;
     

        #endregion


        #region Cotor

        public BookNodeFactory(
                               CaptchaSettings captchaSettings
                              ,CustomerSettings customerSettings
                              ,IAiBookService aiBookService
                              ,ICustomerService customerService
                              ,IDateTimeHelper dateTimeHelper
                              ,IGenericAttributeService genericAttributeService
                              ,IPictureService pictureService
                              ,IStaticCacheManager cacheManager
                              ,IStoreContext storeContext
                              ,IWorkContext workContext
                              ,ICategoryModelFactory categoryModelFactory
                              ,IBaseAdminModelFactory baseAdminModelFactory
                              ,MediaSettings mediaSettings
                              ,IProductService productService
                              ,IBookDirFactory bookdirFactory
                              ,IBookDirService bookdirService
                              ,ILocalizationService localizationService
                              ,ILocalizedModelFactory localizedModelFactory
                                )
        {
            _captchaSettings = captchaSettings;
            _customerSettings = customerSettings;
            _aiBookService = aiBookService;
            _customerService = customerService;
            _dateTimeHelper = dateTimeHelper;
            _genericAttributeService = genericAttributeService;
            _pictureService = pictureService;
            _cacheManager = cacheManager;
            _storeContext = storeContext;
            _workContext = workContext;
            _mediaSettings = mediaSettings;
            _categoryModelFactory = categoryModelFactory;
            _baseAdminModelFactory = baseAdminModelFactory;
            _bookDirFactory = bookdirFactory;
            _bookdirService = bookdirService;
            _localizationService = localizationService;
            _productService = productService;
            _localizedModelFactory = localizedModelFactory;
        }

        #endregion

        public AiBookModelView PrepareBookNodeModel(AiBookModelView bookModelModel, int? filterByBookId)
        {

            var model = new AiBookModel();
            if (bookModelModel == null)
                throw new ArgumentNullException(nameof(bookModelModel));

            Action<AiBookModelLocalizedModel, int> localizedModelConfiguration = null;
            //prepare nested search models
            //Prepare Categories
            _baseAdminModelFactory.PrepareCategories(bookModelModel.AvailableCategories,
               defaultItemText: _localizationService.GetResource("Admin.Catalog.Categories.Fields.Parent.None"));
            if (bookModelModel.BookDirId > 0)
            {
                var bookdir = _bookdirService.GetBookDirById(bookModelModel.BookDirId);
                if (bookdir != null)
                {
                    bookModelModel.BookId = bookdir.BookID;
                }
                bookModelModel.AvailableBookDirs = SelectListHelper.GetBookDirList(_bookdirService, _workContext.CurrentCustomer, _cacheManager);
            }


            if (bookModelModel.BookId > 0)
            {
                var result = _productService.GetProductById(bookModelModel.BookId);
                if (result != null)
                {
                    if (result.ProductCategories != null)
                    {
                        bookModelModel.CateId = result.ProductCategories.FirstOrDefault().CategoryId;
                    }
                    bookModelModel.BookId = result.Id;
                  bookModelModel.AvailableBooks =  SelectListHelper.GetBookList(_productService, new List<int> { bookModelModel.CateId },_workContext.CurrentCustomer,_cacheManager);  
                }
            }
           

            localizedModelConfiguration = (locale, languageId) =>
            {
                locale.Name = _localizationService.GetLocalized(model, entity => entity.Name, languageId, false, false);
                locale.Desc = _localizationService.GetLocalized(model, entity => entity.Desc, languageId, false, false);
                locale.WebModelUrl = _localizationService.GetLocalized(model, entity => entity.WebModelUrl, languageId, false, false);
                locale.WebGltfUrl = _localizationService.GetLocalized(model, entity => entity.WebGltfUrl, languageId, false, false);
                locale.WebBinUrl = _localizationService.GetLocalized(model, entity => entity.WebBinUrl, languageId, false, false);
                locale.AbUrl = _localizationService.GetLocalized(model, entity => entity.AbUrl, languageId, false, false);
                locale.ImgUrl = _localizationService.GetLocalized(model, entity => entity.ImgUrl, languageId, false, false);
                locale.StrJson = _localizationService.GetLocalized(model, entity => entity.StrJson, languageId, false, false);
                //locale.SeName = _urlRecordService.GetSeName(category, languageId, false, false);
            };

           
                bookModelModel.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);
            return bookModelModel;
        }

        public AiBookModelListView PrepareBookNodeListModel(AiBookSearchModelView searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));




            var result = _aiBookService.SearchAiBookModels(searchModel.BookAiModelName,searchModel.Page -1,searchModel.PageSize,new List<int> { searchModel.CateId },searchModel.BookId,searchModel.BookDirId,0);

            var model = new AiBookModelListView().PrepareToGrid(searchModel, result, () =>
            {
                return result.Select(category =>
                {
                    //fill in model values from the entity
                    var categoryModel = category.ToModel<AiBookModelView>();
                    //fill in additional values (not existing in the entity)
                    //categoryModel.Breadcrumb = _bookDirService.GetFormattedBreadCrumb(category);
                    //categoryModel.SeName = _urlRecordService.GetSeName(category, 0, true, false);
                    return categoryModel;
                });
            });


            return model;
           // _baseAdminModelFactory.PrepareCategories(searchModel.AvailableCategories,
             // defaultItemText: _localizationService.GetResource("Admin.Catalog.Categories.Fields.Parent.None"));


            //if (searchModel.BookId > 0)
            //{
            //    var result = _productService.GetProductById(searchModel.BookId);
            //    if (result != null)
            //    {
            //        if (result.ProductCategories != null)
            //        {
            //            searchModel.CateId = result.ProductCategories.FirstOrDefault().CategoryId;
            //        }
            //        searchModel.BookId = result.Id;
            //        searchModel.AvailableBooks = SelectListHelper.GetBookList(_productService, new List<int> { searchModel.CateId });
            //    }
            //}
            //if (searchModel.BookDirId > 0)
            //{
            //    searchModel.AvailableBookDirs = SelectListHelper.GetBookDirList(_bookdirService);
            //}
            //return searchModel;
            // throw new NotImplementedException();
        }

        public AiBookSearchModelView PrepareBookNodeSearchModel(AiBookSearchModelView searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));


            _baseAdminModelFactory.PrepareCategories(searchModel.AvailableCategories,
              defaultItemText: _localizationService.GetResource("Admin.Catalog.Categories.Fields.Parent.None"));
            if (searchModel.BookDirId > 0)
            {

               var bookdir = _bookdirService.GetBookDirById(searchModel.BookDirId);

                if (bookdir != null)
                {
                    searchModel.BookId = bookdir.BookID;
                }
                searchModel.AvailableBookDirs = SelectListHelper.GetBookDirList(_bookdirService);
            }

            if (searchModel.BookId > 0)
            {
                var result = _productService.GetProductById(searchModel.BookId);
                if (result != null)
                {
                    if (result.ProductCategories != null)
                    {
                        searchModel.CateId = result.ProductCategories.FirstOrDefault().CategoryId;
                    }
                    searchModel.BookId = result.Id;
                    searchModel.AvailableBooks = SelectListHelper.GetBookList(_productService, new List<int> { searchModel.CateId });
                }
            }
     
            return searchModel;
        }
    }
}
