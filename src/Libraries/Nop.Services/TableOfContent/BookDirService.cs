using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.TableOfContent;
using Nop.Services.Events;

namespace Nop.Services.TableOfContent
{
    public partial class BookDirService : IBookDirService
    {

        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<BookDir> _bookdirRepository;
        private readonly IStaticCacheManager _cacheManager;
        //private readonly ICacheManager

        #endregion


        #region  Ctor

        public BookDirService(IEventPublisher eventPublisher,
            IRepository<BookDir> bookdirRepository,
            IStaticCacheManager cacheManager)
        {
            _eventPublisher = eventPublisher;
            _bookdirRepository = bookdirRepository;
            _cacheManager = cacheManager;
        }
        #endregion

        public int DeleteBookDir(BookDir store)
        {
            throw new NotImplementedException();
        }

        public IList<BookDir> GetAllBookDirs(bool loadCacheableCopy = true)
        {
            throw new NotImplementedException();
        }

        public BookDir GetBookDirById(int storeId, bool loadCacheableCopy = true)
        {
            throw new NotImplementedException();
        }

        public string[] GetNotExistingBookDirs(string[] storeIdsNames)
        {
            throw new NotImplementedException();
        }

        public int InsertBookDir(BookDir bookdir)
        {
            throw new NotImplementedException();
        }

        public int UpdateBookDir(BookDir bookdir)
        {
            throw new NotImplementedException();
        }
    }
}
