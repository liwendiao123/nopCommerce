using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.AIBookModel;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Stores;
using Nop.Data;
using Nop.Services.Catalog;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.TableOfContent;
using Nop.Services.Logging;
using Nop.Core.Domain.TableOfContent;
using System.Linq;
namespace Nop.Services.AIBookModel
{
    public partial class AiBookService : IAiBookService
    {

        #region Fields
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<AiBookModel> _bookNodeRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IStaticCacheManager _cacheManager;
        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        private readonly CommonSettings _commonSettings;
        private readonly IAclService _aclService;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly ILocalizationService _localizationService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IStoreContext _storeContext;
        private readonly ICategoryService _cateservice;
        private readonly IProductService _productService;
        private readonly IBookDirService _bookDirService;
        #endregion


        public AiBookService(
                        IEventPublisher eventPublisher,
                        IRepository<AiBookModel> bookNodeRepository,
                        IRepository<StoreMapping> storeMappingRepository,
                        IStaticCacheManager cacheManager,
                        ILogger logger,
                        IWorkContext workContext,
                        CommonSettings commonSettings,
                        IAclService aclService,
                        IDataProvider dataProvider,
                        IDbContext dbContext,
                        ILocalizationService localizationService,
                        IStaticCacheManager staticCacheManager,
                        IStoreContext storeContext,
                        ICategoryService cateservice,
                        IProductService productService,
                        IBookDirService bookDirService
            )
        {

            _eventPublisher = eventPublisher;
            _bookNodeRepository = bookNodeRepository;
            _storeMappingRepository = storeMappingRepository;
            _cacheManager = cacheManager;
            _logger = logger;
            _workContext = workContext;
            _commonSettings = commonSettings;
            _aclService = aclService;
            _dataProvider = dataProvider;
            _dbContext = dbContext;
            _localizationService = localizationService;
            _staticCacheManager = staticCacheManager;
            _storeContext = storeContext;
            _cateservice = cateservice;
            _productService = productService;
            _bookDirService = bookDirService;

        }

        public int DeleteAiBookModel(AiBookModel store)
        {
            try
            {
                if (store == null)
                    throw new ArgumentNullException(nameof(store));

                if (store is IEntityForCaching)
                    throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

               // var allStores = GetAllBookDirs();
              //  if (allStores.Count == 1)
                //    throw new Exception("You cannot delete the only configured BookDir");

                _bookNodeRepository.Delete(store);

                _cacheManager.RemoveByPrefix(NopBookNodeDefault.BookNodesPrefixCacheKey);

                //event notification
                _eventPublisher.EntityDeleted(store);

                return 1;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex, _workContext.CurrentCustomer);


                return 0;
            }

        }

        public AiBookModel GetAiBookModelById(int booknodeId, bool loadCacheableCopy = true)
        {
            if (booknodeId == 0)
                return null;

            AiBookModel LoadStoreFunc()
            {
                return _bookNodeRepository.GetById(booknodeId);
            }

            if (!loadCacheableCopy)
                return LoadStoreFunc();

            //cacheable copy
            var key = string.Format(NopBookNodeDefault.BookNodesByIdCacheKey, booknodeId);
            return _cacheManager.Get(key, () =>
            {
                var store = LoadStoreFunc();
                if (store == null)
                    return null;
                return store;
            });
        }

        public IList<AiBookModel> GetAllAiBookModels(bool loadCacheableCopy = true)
        {
            IList<AiBookModel> loadBookDirsFunc()
            {
                var query = from s in _bookNodeRepository.Table orderby s.DisplayOrder, s.Id select s;
                return query.ToList();
            }

            if (loadCacheableCopy)
            {
                //cacheable copy
                return _cacheManager.Get(NopBookNodeDefault.BookNodesAllCacheKey, () =>
                {
                    var result = new List<AiBookModel>();
                    foreach (var store in loadBookDirsFunc())
                        result.Add(store);
                    return result;
                });
            }
            return loadBookDirsFunc();
            //throw new NotImplementedException();
        }

        public string[] GetNotExistingAiBookModels(string[] storeIdsNames)
        {
            if (storeIdsNames == null)
                throw new ArgumentNullException(nameof(storeIdsNames));

            var query = _bookNodeRepository.Table;
            var queryFilter = storeIdsNames.Distinct().ToArray();
            //filtering by name
            var filter = query.Select(store => store.Name).Where(store => queryFilter.Contains(store)).ToList();
            queryFilter = queryFilter.Except(filter).ToArray();

            //if some names not found
            if (!queryFilter.Any())
                return queryFilter.ToArray();

            //filtering by IDs
            filter = query.Select(store => store.Id.ToString()).Where(store => queryFilter.Contains(store)).ToList();
            queryFilter = queryFilter.Except(filter).ToArray();

            return queryFilter.ToArray();
        }

        public int InsertAiBookModel(AiBookModel bookNode)
        {
            try
            {
                if (bookNode == null)
                    throw new ArgumentNullException(nameof(bookNode));

                if (bookNode is IEntityForCaching)
                    throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

                //_bookdirRepository.Insert(bookNode);
                _bookNodeRepository.Insert(bookNode);
                _cacheManager.RemoveByPrefix(NopBookDirDefault.BookDirsPrefixCacheKey);

                //event notification
                _eventPublisher.EntityInserted(bookNode);


                return 1;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex, _workContext.CurrentCustomer);

                return 0;
            }
        }

        public IPagedList<AiBookModel> SearchAiBookModels(string aibookNodeName,int pageIndex = 0, int pageSize = int.MaxValue, IList<int> categoryIds = null, int bookId = 0, int bookdirId = 0, int vendorId = 0, bool visibleIndividuallyOnly = false,  string keywords = null,     bool showHidden = false)
        {


            var query = _bookNodeRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.Published);
            if (!string.IsNullOrWhiteSpace(aibookNodeName))
                query = query.Where(c => c.Name.Contains(aibookNodeName));
            query = query.Where(c => !c.Deleted);

            #region 短路条件查询 ----范围由小到大提高查询效率 liwendiao 备注
            if (bookdirId > 0)
            {
                var resultIds =_bookDirService. GetChildBookDirIds(bookdirId);
                query = query.Where(x => resultIds.Contains(x.BookDirID));
            }

            else if (bookId > 0)
            {

               // _bookDirService.ge

                query = query.Where(x => bookId == x.bo);
            }
            else if (cateId > 0)
            {
                ///todo..
                var result = _cateservice.GetChildCategoryIds(cateId);
                if (result != null && !result.Contains(cateId))
                {
                    result.Add(cateId);
                }
                if (result == null)
                {
                    result = new List<int>();
                }
                var product = _productService.SearchProducts(0, Int32.MaxValue, result);
                if (product != null)
                {
                    var pres = product.OrderBy(x => x.Id).Select(x => x.Id).ToList();
                    query = query.Where(x => pres.Contains(x.BookID));
                }
            }

            #endregion


            throw new NotImplementedException();
        }

        public int UpdateAiBookModel(AiBookModel aibookmodel)
        {
            try
            {
                if (aibookmodel == null)
                    throw new ArgumentNullException(nameof(aibookmodel));

                if (aibookmodel is IEntityForCaching)
                    throw new ArgumentException("Cacheable entities are not supported by Entity Framework");
                _bookNodeRepository.Update(aibookmodel);
                _cacheManager.RemoveByPrefix(NopBookNodeDefault.BookNodesPrefixCacheKey);
                //event notification
                _eventPublisher.EntityUpdated(aibookmodel);
                return 1;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex, _workContext.CurrentCustomer);
                return 0;
            }
        }


        public bool CheckExist(AiBookModel aibookmodel)
        {
            return false;
        }
    }
}
