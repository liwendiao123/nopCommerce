using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.TableOfContent;
using Nop.Services.Events;
using System.Linq;
using Nop.Services.Logging;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Services.Security;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Core.Domain.Stores;

namespace Nop.Services.TableOfContent
{
    public partial class BookDirService : IBookDirService
    {

        #region Fields
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<BookDir> _bookdirRepository;
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

        #endregion


        #region  Ctor

        public BookDirService(
            IEventPublisher eventPublisher,
            IRepository<BookDir> bookdirRepository,
            IStaticCacheManager cacheManager, 
            ILogger logger, 
            IWorkContext workContext,
            CommonSettings commonSettings
            )
        {
            _eventPublisher = eventPublisher;
            _bookdirRepository = bookdirRepository;
            _cacheManager = cacheManager;
            _logger = logger;
            _workContext = workContext;
        }
        #endregion

        public int DeleteBookDir(BookDir store)
        {


            try
            {
                if (store == null)
                    throw new ArgumentNullException(nameof(store));

                if (store is IEntityForCaching)
                    throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

                var allStores = GetAllBookDirs();
                if (allStores.Count == 1)
                    throw new Exception("You cannot delete the only configured BookDir");

                _bookdirRepository.Delete(store);

                _cacheManager.RemoveByPrefix(NopBookDirDefault.BookDirsPrefixCacheKey);

                //event notification
                _eventPublisher.EntityDeleted(store);

                return 1;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message,ex, _workContext.CurrentCustomer);


                return 0;
            }
           


        }

        public IList<BookDir> GetAllBookDirs(bool loadCacheableCopy = true)
        {



            IList<BookDir> loadBookDirsFunc()
            {
                var query = from s in _bookdirRepository.Table orderby s.DisplayOrder, s.Id select s;
                return query.ToList();
            }

            if (loadCacheableCopy)
            {
                //cacheable copy
                return _cacheManager.Get(NopBookDirDefault.BookDirsAllCacheKey, () =>
                {
                    var result = new List<BookDir>();
                    foreach (var store in loadBookDirsFunc())
                        result.Add(store);
                    return result;
                });
            }

            return loadBookDirsFunc();
        }


        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="categoryName">Category name</param>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        public virtual IPagedList<BookDir> GetAllBookDirsData(string categoryName, int storeId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            //if (_commonSettings.UseStoredProcedureForLoadingCategories)
            //{
            //    //stored procedures are enabled for loading categories and supported by the database. 
            //    //It's much faster with a large number of categories than the LINQ implementation below 

            //    //prepare parameters
            //    var showHiddenParameter = _dataProvider.GetBooleanParameter("ShowHidden", showHidden);
            //    var nameParameter = _dataProvider.GetStringParameter("Name", categoryName ?? string.Empty);
            //    var storeIdParameter = _dataProvider.GetInt32Parameter("StoreId", !_catalogSettings.IgnoreStoreLimitations ? storeId : 0);
            //    var pageIndexParameter = _dataProvider.GetInt32Parameter("PageIndex", pageIndex);
            //    var pageSizeParameter = _dataProvider.GetInt32Parameter("PageSize", pageSize);
            //    //pass allowed customer role identifiers as comma-delimited string
            //    var customerRoleIdsParameter = _dataProvider.GetStringParameter("CustomerRoleIds", !_catalogSettings.IgnoreAcl ? string.Join(",", _workContext.CurrentCustomer.GetCustomerRoleIds()) : string.Empty);

            //    var totalRecordsParameter = _dataProvider.GetOutputInt32Parameter("TotalRecords");

            //    //invoke stored procedure
            //    var categories = _dbContext.EntityFromSql<Category>("CategoryLoadAllPaged",
            //        showHiddenParameter, nameParameter, storeIdParameter, customerRoleIdsParameter,
            //        pageIndexParameter, pageSizeParameter, totalRecordsParameter).ToList();
            //    var totalRecords = totalRecordsParameter.Value != DBNull.Value ? Convert.ToInt32(totalRecordsParameter.Value) : 0;

            //    //paging
            //    return new PagedList<Category>(categories, pageIndex, pageSize, totalRecords);
            //}

            //don't use a stored procedure. Use LINQ
            var query = _bookdirRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.Published);
            if (!string.IsNullOrWhiteSpace(categoryName))
                query = query.Where(c => c.Name.Contains(categoryName));
            query = query.Where(c => !c.Deleted);
            query = query.OrderBy(c => c.ParentBookDirId).ThenBy(c => c.DisplayOrder).ThenBy(c => c.Id);

            //if ((storeId > 0 && !_catalogSettings.IgnoreStoreLimitations) || (!showHidden && !_catalogSettings.IgnoreAcl))
            //{
            //    //if (!showHidden && !_catalogSettings.IgnoreAcl)
            //    //{
            //    //    //ACL (access control list)
            //    //    var allowedCustomerRolesIds = _workContext.CurrentCustomer.GetCustomerRoleIds();
            //    //    query = from c in query
            //    //            join acl in _aclRepository.Table
            //    //                on new { c1 = c.Id, c2 = nameof(Category) } equals new { c1 = acl.EntityId, c2 = acl.EntityName } into c_acl
            //    //            from acl in c_acl.DefaultIfEmpty()
            //    //            where !c.SubjectToAcl || allowedCustomerRolesIds.Contains(acl.CustomerRoleId)
            //    //            select c;
            //    //}

            //    //if (storeId > 0 && !_catalogSettings.IgnoreStoreLimitations)
            //    //{
            //    //    //Store mapping
            //    //    query = from c in query
            //    //            join sm in _storeMappingRepository.Table
            //    //                on new { c1 = c.Id, c2 = nameof(Category) } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into c_sm
            //    //            from sm in c_sm.DefaultIfEmpty()
            //    //            where !c.LimitedToStores || storeId == sm.StoreId
            //    //            select c;
            //    //}

            //    query = query.Distinct().OrderBy(c => c.ParentBookDirId).ThenBy(c => c.DisplayOrder).ThenBy(c => c.Id);
            //}

            var unsortedCategories = query.ToList();

            //sort categories
            var sortedCategories = SortBookDirsForTree(unsortedCategories);

            //paging
            return new PagedList<BookDir>(sortedCategories, pageIndex, pageSize);
        }


        /// <summary>
        /// Sort categories for tree representation
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="parentId">Parent category identifier</param>
        /// <param name="ignoreCategoriesWithoutExistingParent">A value indicating whether categories without parent category in provided category list (source) should be ignored</param>
        /// <returns>Sorted categories</returns>
        public virtual IList<BookDir> SortBookDirsForTree(IList<BookDir> source, int parentId = 0,
            bool ignoreCategoriesWithoutExistingParent = false)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var result = new List<BookDir>();

            foreach (var cat in source.Where(c => c.ParentBookDirId == parentId).ToList())
            {
                result.Add(cat);
                result.AddRange(SortBookDirsForTree(source, cat.Id, true));
            }

            if (ignoreCategoriesWithoutExistingParent || result.Count == source.Count)
                return result;

            //find categories without parent in provided category source and insert them into result
            foreach (var cat in source)
                if (result.FirstOrDefault(x => x.Id == cat.Id) == null)
                    result.Add(cat);

            return result;
        }

        public BookDir GetBookDirById(int storeId, bool loadCacheableCopy = true)
        {
            if (storeId == 0)
                return null;

            BookDir LoadStoreFunc()
            {
                return _bookdirRepository.GetById(storeId);
            }

            if (!loadCacheableCopy)
                return LoadStoreFunc();

            //cacheable copy
            var key = string.Format(NopBookDirDefault.BookDirsByIdCacheKey, storeId);
            return _cacheManager.Get(key, () =>
            {
                var store = LoadStoreFunc();
                if (store == null)
                    return null;
                return store;
            });
        }

        public string[] GetNotExistingBookDirs(string[] storeIdsNames)
        {
            if (storeIdsNames == null)
                throw new ArgumentNullException(nameof(storeIdsNames));

            var query = _bookdirRepository.Table;
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

        public int InsertBookDir(BookDir bookdir)
        {

            try
            {
                if (bookdir == null)
                    throw new ArgumentNullException(nameof(bookdir));

                if (bookdir is IEntityForCaching)
                    throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

                _bookdirRepository.Insert(bookdir);

                _cacheManager.RemoveByPrefix(NopBookDirDefault.BookDirsPrefixCacheKey);

                //event notification
                _eventPublisher.EntityInserted(bookdir);


                return 1;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex,_workContext.CurrentCustomer);

                return 0;
            }
         
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookdir"></param>
        /// <returns></returns>
        public int UpdateBookDir(BookDir bookdir)
        {

            try
            {
                if (bookdir == null)
                    throw new ArgumentNullException(nameof(bookdir));

                if (bookdir is IEntityForCaching)
                    throw new ArgumentException("Cacheable entities are not supported by Entity Framework");

                _bookdirRepository.Update(bookdir);

                _cacheManager.RemoveByPrefix(NopBookDirDefault.BookDirsPrefixCacheKey);

                //event notification
                _eventPublisher.EntityUpdated(bookdir);


                return 1;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex,_workContext.CurrentCustomer);
                return 0;
            }
           
        }

        public string GetFormattedBreadCrumb(BookDir bookDir, IList<BookDir> allBookDirs = null,
            string separator = ">>", int languageId = 0)
        {
            var result = string.Empty;

            var breadcrumb = GetBookDirBreadCrumb(bookDir, allBookDirs, true);
            for (var i = 0; i <= breadcrumb.Count - 1; i++)
            {
                var categoryName = _localizationService.GetLocalized(breadcrumb[i], x => x.Name, languageId);
                result = string.IsNullOrEmpty(result) ? categoryName : $"{result} {separator} {categoryName}";
            }

            return result;
        }

      public  IList<BookDir> GetBookDirBreadCrumb(BookDir bookDir, IList<BookDir> allBookDirs = null, bool showHidden = false)
        {
            if (bookDir == null)
                throw new ArgumentNullException(nameof(bookDir));

            var result = new List<BookDir>();

            //used to prevent circular references
            var alreadyProcessedCategoryIds = new List<int>();

            while (bookDir != null && //not null
                !bookDir.Deleted && //not deleted
                (showHidden || bookDir.Published) && //published
                (showHidden || _aclService.Authorize(bookDir)) && //ACL
               // (showHidden || _storeMappingService.Authorize(bookDir)) && //Store mapping
                !alreadyProcessedCategoryIds.Contains(bookDir.Id)) //prevent circular references
            {
                result.Add(bookDir);
                alreadyProcessedCategoryIds.Add(bookDir.Id);
                bookDir = allBookDirs != null ? allBookDirs.FirstOrDefault(c => c.Id == bookDir.ParentBookDirId)
                    : GetBookDirById( bookDir.ParentBookDirId );
            }
            result.Reverse();
            return result;
           // return new List<BookDir>();
        }

       // IList<BookDir> GetBookDirBreadCrumb(BookDir bookDir, IList<BookDir> allBookDirs = null, bool showHidden = false)
    }
}
