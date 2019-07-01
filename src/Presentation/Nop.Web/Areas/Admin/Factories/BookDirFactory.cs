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
using Nop.Web.Framework.Models.Extensions;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Core.Domain.TableOfContent;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Catalog;
using Nop.Services;
using Nop.Core.Domain.Gdpr;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial class BookDirFactory : IBookDirFactory
    {

        #region Fields
        private readonly IAclSupportedModelFactory _aclSupportedModelFactory;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
      //  private readonly IBookDirFactory _bookDirFactory;
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
            ,IProductService productService
            , IUrlRecordService urlRecordService)
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
            _productService = productService;
           // _bookDirFactory = bookDirFactory;
        }
        public BookDirListModel PrepareBookDirListModel(BookDirSearchModel searchModel)
        {

            if (searchModel == null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

           var result =   _bookDirService.GetAllBookDirsData(searchModel.SearchDirName,searchModel.CategoryID,searchModel.BookID,searchModel.BookDirId,searchModel.SearchStoreId, searchModel.Page-1, searchModel.PageSize);


            var model = new BookDirListModel().PrepareToGrid(searchModel, result, () =>
            {
                return result.Select(category =>
                {
                    //fill in model values from the entity
                    var categoryModel = category.ToModel<BookDirModel>();
                    //fill in additional values (not existing in the entity)
                    categoryModel.Breadcrumb = _bookDirService.GetFormattedBreadCrumb(category);
                    categoryModel.SeName = _urlRecordService.GetSeName(category, 0, true, false);
                    return categoryModel;
                });
            });


            return model;
        }
        public BookDirModel PrepareBookDirModel(BookDirModel model, BookDir bookdir = null, bool excludeProperties = false)
        {
            Action<BookDirLocalizedModel, int> localizedModelConfiguration = null;

            if (bookdir != null)
            {
                //fill in model values from the entity
                if (model == null)
                {
                    model = bookdir.ToModel<BookDirModel>();
                    model.SeName = _urlRecordService.GetSeName(bookdir, 0, true, false);
                }

                var result = _productService.GetProductById(bookdir.BookID);

                if (result != null )
                {
                    if (result.ProductCategories != null)
                    {
                        model.CategryID = result.ProductCategories.FirstOrDefault().CategoryId;
                    }                
                    model.BookID = result.Id;


                    var products = _productService.SearchProducts(showHidden: true,
                                    categoryIds: new List<int>() {
                                        model.CategryID
                                    },
                                    manufacturerId: 0,
                                    storeId: 0,
                                    vendorId: 0,
                                    warehouseId: 0,
                                    productType: null,
                                    keywords:null,
                                    pageIndex: 0, 
                                    pageSize: int.MaxValue);

                   // model.BookList = products.ToList().ToSelect<Product>()
                }
                //prepare nested search model

               // PrepareBookDirSearchModel()

              //  PrepareCategoryProductSearchModel(model.CategoryProductSearchModel, category);

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(bookdir, entity => entity.Name, languageId, false, false);
                    locale.Description = _localizationService.GetLocalized(bookdir, entity => entity.Description, languageId, false, false);
                    locale.MetaKeywords = _localizationService.GetLocalized(bookdir, entity => entity.MetaKeywords, languageId, false, false);
                    locale.MetaDescription = _localizationService.GetLocalized(bookdir, entity => entity.MetaDescription, languageId, false, false);
                    //locale.MetaTitle = _localizationService.GetLocalized(category, entity => entity.MetaTitle, languageId, false, false);
                    //locale.SeName = _urlRecordService.GetSeName(category, languageId, false, false);
                };
            }

            //set default values for the new model
            if (bookdir == null)
            {

                if (model == null)
                {
                    model = new BookDirModel();
                }

                //model.PageSize = _catalogSettings.DefaultCategoryPageSize;
               // model.PageSizeOptions = _catalogSettings.DefaultCategoryPageSizeOptions;
                model.Published = true;
              //  model.IncludeInTopMenu = true;
                model.AllowCustomersToSelectPageSize = true;
            }

            //prepare localized models
            if (!excludeProperties)
               model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            //prepare available category templates
           // _baseAdminModelFactory.PrepareCategoryTemplates(model.AvailableCategoryTemplates, false);
            //prepare available parent categories
           _baseAdminModelFactory.PrepareCategories(model.AvailableCategories,
                defaultItemText: _localizationService.GetResource("Admin.Catalog.Categories.Fields.Parent.None"));
            //prepare model discounts
           // var availableDiscounts = _discountService.GetAllDiscounts(DiscountType.AssignedToCategories, showHidden: true);
            //_discountSupportedModelFactory.PrepareModelDiscounts(model, category, availableDiscounts, excludeProperties);
            //prepare model customer roles
            _aclSupportedModelFactory.PrepareModelCustomerRoles(model, bookdir, excludeProperties);
            //prepare model stores
           _storeMappingSupportedModelFactory.PrepareModelStores(model, bookdir, excludeProperties);
            return model;
        }

        public BookDirModel PrepareBookDirModel()
        {


          return  PrepareBookDirModel(new BookDirModel());

            ///throw new NotImplementedException();
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



        public virtual void PrepareBookItems(IList<SelectListItem> items,IList<Product> bitems,bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            //if (items == null)
            //    throw new ArgumentNullException(nameof(items));

            ////prepare available request types
            //var gdprRequestTypeItems = GdprRequestType.ConsentAgree.ToSelectList(false);
            //foreach (var gdprRequestTypeItem in bitems)
            //{
            //    items.Add(gdprRequestTypeItem);
            //}

            ////insert special item for the default value
            //PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }
    }
}
        