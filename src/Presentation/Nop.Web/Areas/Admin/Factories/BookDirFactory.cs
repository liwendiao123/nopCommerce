using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Areas.Admin.Models.TableOfContent;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial class BookDirFactory : IBookDirFactory
    {

        #region Fields

        //private readonly CatalogSettings _catalogSettings;
        //private readonly IAclSupportedModelFactory _aclSupportedModelFactory;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;


        //private readonly ICategoryService _categoryService;
        //private readonly IDiscountService _discountService;
        //private readonly IDiscountSupportedModelFactory _discountSupportedModelFactory;
        //private readonly ILocalizationService _localizationService;
        //private readonly ILocalizedModelFactory _localizedModelFactory;
        //private readonly IProductService _productService;
        //private readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;
        //private readonly IUrlRecordService _urlRecordService;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public BookDirFactory()
        {

        }



        public BookDirListModel PrepareBookDirListModel()
        {
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

            //prepare available stores
            _baseAdminModelFactory.PrepareStores(searchModel.AvailableStores);

            //prepare page parameters
            searchModel.SetGridPageSize();
            return searchModel;
        }
    }
}
