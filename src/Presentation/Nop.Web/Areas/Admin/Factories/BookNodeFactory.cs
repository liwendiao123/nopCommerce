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
        }

        #endregion

        public AiBookModelView PrepareBookNodeModel(AiBookModelView bookModelModel, int? filterByBookId)
        {
            if (bookModelModel == null)
                throw new ArgumentNullException(nameof(bookModelModel));
            //prepare nested search models
            //Prepare Categories
            _baseAdminModelFactory.PrepareCategories(bookModelModel.AvailableCategories,
               defaultItemText: _localizationService.GetResource("Admin.Catalog.Categories.Fields.Parent.None"));
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
                  bookModelModel.AvailableBooks =  SelectListHelper.GetBookList(_productService, new List<int> { bookModelModel.CateId });  
                }
            }
            if (bookModelModel.BookDirId > 0)
            {
              bookModelModel.AvailableBookDirs =  SelectListHelper.GetBookDirList(_bookdirService);
            }


            return bookModelModel;
        }

        public AiBookSearchModelView PrepareBookNodeListModel(AiBookSearchModelView searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));


            _baseAdminModelFactory.PrepareCategories(searchModel.AvailableCategories,
              defaultItemText: _localizationService.GetResource("Admin.Catalog.Categories.Fields.Parent.None"));


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
            if (searchModel.BookDirId > 0)
            {
                searchModel.AvailableBookDirs = SelectListHelper.GetBookDirList(_bookdirService);
            }
            return searchModel;
            // throw new NotImplementedException();
        }

        public AiBookSearchModelView PrepareBlogPostSearchModel(AiBookSearchModelView searchModel)
        {
            throw new NotImplementedException();
        }
    }
}
