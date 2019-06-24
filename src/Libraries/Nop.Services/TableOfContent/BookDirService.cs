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

namespace Nop.Services.TableOfContent
{
    public partial class BookDirService : IBookDirService
    {

        #region Fields
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<BookDir> _bookdirRepository;
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
            CommonSettings _commonSettings
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

        IList<BookDir> GetBookDirBreadCrumb(BookDir bookDir, IList<BookDir> allBookDirs = null, bool showHidden = false)
        {



            return new List<BookDir>();
        }
    }
}
