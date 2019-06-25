using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Discounts;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Services.TableOfContent;
using Nop.Web.Areas.Admin.Models.TableOfContent;
using Nop.Web.Framework.Factories;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial class BookDirFactory : IBookDirFactory
    {

        #region Fields
        private readonly IAclSupportedModelFactory _aclSupportedModelFactory;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IBookDirFactory _bookDirFactory;
        private readonly IBookDirService _bookDirService;
        private readonly ICategoryService _categoryService;
        private readonly ICurrencyService _currencyService;
        private readonly ICustomerService _customerService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IManufacturerService _manufacturerService;
        private readonly IMeasureService _measureService;       
        private readonly IPictureService _pictureService;
        private readonly IProductAttributeFormatter _productAttributeFormatter;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IProductService _productService;
        private readonly IProductTagService _productTagService;
        private readonly IProductTemplateService _productTemplateService;
        private readonly ISettingModelFactory _settingModelFactory;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;
        private readonly IStoreService _storeService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;
        #endregion
        /// <summary>
        /// 构造函数
        /// </summary>
        public BookDirFactory(IBaseAdminModelFactory baseAdminModelFactory
            ,IAclSupportedModelFactory aclSupportedModelFactory
            ,ICategoryService categoryService
            ,ICurrencyService currencyService
            ,ICustomerService customerService
            ,IDateTimeHelper dateTimeHelper
            ,ILocalizationService localizationService
            ,ILocalizedModelFactory localizedModelFactory
            ,IManufacturerService manufacturerService
            ,IMeasureService measureService
            ,IStoreService storeService
            ,IPictureService pictureService
            ,IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory
            ,IWorkContext workContext      
            ,IBookDirService bookDirService
            ,IBookDirFactory bookDirFactory
            ,IUrlRecordService urlRecordService)
        {
            _baseAdminModelFactory = baseAdminModelFactory;
            _aclSupportedModelFactory = aclSupportedModelFactory;
            _categoryService = categoryService;
            _currencyService = currencyService;
            _customerService = customerService;
            _dateTimeHelper = dateTimeHelper;
            _localizationService = localizationService;
            _localizedModelFactory = localizedModelFactory;
            _manufacturerService = manufacturerService;
            _measureService = measureService;
            _storeService = storeService;
            _pictureService = pictureService;
            _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
            _workContext = workContext;
            _bookDirService = bookDirService;
            _urlRecordService = urlRecordService;
            _bookDirFactory = bookDirFactory;
        }
        public BookDirListModel PrepareBookDirListModel()
        {

            try
            {

            }
            catch (Exception ex)
            {

            }
            throw new NotImplementedException();
        }
        public BookDirModel PrepareBookDirModel()
        {
            throw new NotImplementedException();
        }

        public BookDirSearchModel PrepareBookDirSearchModel(BookDirSearchModel searchModel, BookDirModel bdm)
        {

            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            searchModel.SearchDirName = bdm.Name;




          ///  searchModel.AvailableBooks = 

            //prepare available stores
            _baseAdminModelFactory.PrepareStores(searchModel.AvailableStores);


            _baseAdminModelFactory.PrepareCategories(searchModel.AvailableCategories);

           //searchModel.AvailableBooks = _productService.ge


            //prepare page parameters
            searchModel.SetGridPageSize();
            return searchModel;
        }
    }
}
        