using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.AIBookModel;

using Nop.Data;
using Nop.Services.Events;
using Nop.Services.Seo;
using System.Linq;
using Nop.Core.Data.Extensions;

using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Catalog;

namespace Nop.Services.AIBookModel
{
    public class BookNodeTagService : IBookNodeTagService
    {


        #region  字段
        private readonly CatalogSettings _catalogSettings;
        private readonly ICacheManager _cacheManager;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IAiBookService _aiBookService;
        private readonly IRepository<BookNodeBookNodeTagMapping> _bookNodeBookNodeTagMappingRepository;
        private readonly IRepository<BookNodeTag> _bookNodeTagRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;
        #endregion


        #region   构造函数
        public BookNodeTagService(
            CatalogSettings catalogSettings,
            ICacheManager cacheManager,
            IDataProvider dataProvider,
            IDbContext dbContext,
            IEventPublisher eventPublisher,
            IAiBookService aiBookService,
            IRepository<BookNodeBookNodeTagMapping> bookNodeBookNodeTagMappingRepository,
            IRepository<BookNodeTag> bookNodeTagRepository,
            IStaticCacheManager staticCacheManager,
            IUrlRecordService urlRecordService,
            IWorkContext workContext
            )
        {
            _catalogSettings= catalogSettings;
            _cacheManager =cacheManager;
            _dataProvider= dataProvider;
            _dbContext =dbContext;
            _eventPublisher =eventPublisher;
            _aiBookService =aiBookService;
            _bookNodeBookNodeTagMappingRepository =bookNodeBookNodeTagMappingRepository;
            _bookNodeTagRepository=bookNodeTagRepository;
            _staticCacheManager =staticCacheManager;
            _urlRecordService=urlRecordService;
            _workContext =workContext;
        }
        #endregion

        #region 内部方法

        #endregion 

        #region 执行方法
        public void DeleteBookNodeTag(BookNodeTag bookNodeTag)
        {
            if (bookNodeTag == null)
                throw new ArgumentNullException(nameof(bookNodeTag));

            _bookNodeTagRepository.Delete(bookNodeTag);

            //cache
            _cacheManager.RemoveByPrefix(NopBookNodeDefault.BookNodesTagPrefixCacheKey);
            _staticCacheManager.RemoveByPrefix(NopBookNodeDefault.BookNodesTagPrefixCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(bookNodeTag);
        }

        public IList<BookNodeTag> GetAllBookNodeTags()
        {
            var query = _bookNodeTagRepository .Table;
            var productTags = query.ToList();
            return productTags;
        }

        public IList<BookNodeTag> GetAllBookNodeTagsByBookNodeId(int bookNodeId)
        {
            var key = string.Format(NopBookNodeDefault.BookNodesTagAllByProductIdCacheKey, bookNodeId);
            return _cacheManager.Get(key, () =>
            {
                var query = from pt in _bookNodeTagRepository.Table
                            join ppt in _bookNodeBookNodeTagMappingRepository .Table on pt.Id equals ppt.BookNodeTagId
                            where ppt.BookNodeId == bookNodeId
                            orderby pt.Id
                            select pt;

                var productTags = query.ToList();
                return productTags;
            });
        }

        public int GetBookNodeCount(int bookNodeTagId, int storeId, bool showHidden = false)
        {
            //var dictionary = GetBookNodeCount(storeId, showHidden);
            //if (dictionary.ContainsKey(productTagId))
            //    return dictionary[productTagId];

            return 0;
        }

        public BookNodeTag GetBookNodeTagById(int booknodeTagId)
        {
            if (booknodeTagId == 0)
                return null;

            return _bookNodeTagRepository .GetById(booknodeTagId);
        }

        public BookNodeTag GetBookNodeTagByName(string name)
        {
            var query = from pt in _bookNodeTagRepository.Table
                        where pt.Name == name
                        select pt;

            var productTag = query.FirstOrDefault();
            return productTag;
        }

        public void InsertBookNodeTag(BookNodeTag bookNodeTag)
        {
            if (bookNodeTag == null)
                throw new ArgumentNullException(nameof(bookNodeTag));

            _bookNodeTagRepository .Insert(bookNodeTag);

            //cache
            _cacheManager.RemoveByPrefix(NopBookNodeDefault.BookNodesPrefixCacheKey);
            _staticCacheManager.RemoveByPrefix(NopBookNodeDefault.BookNodesPrefixCacheKey);

            //event notification
            _eventPublisher.EntityInserted(bookNodeTag);
        }

        public void UpdateBookNodeTag(BookNodeTag bookNodeTag)
        {
            if (bookNodeTag == null)
                throw new ArgumentNullException(nameof(bookNodeTag));

            _bookNodeTagRepository .Update(bookNodeTag);

            var seName = _urlRecordService.ValidateSeName(bookNodeTag, string.Empty, bookNodeTag.Name, true);
            _urlRecordService.SaveSlug(bookNodeTag, seName, 0);

            //cache
            _cacheManager.RemoveByPrefix(NopBookNodeDefault.BookNodesPrefixCacheKey);
            _staticCacheManager.RemoveByPrefix(NopBookNodeDefault.BookNodesPrefixCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(bookNodeTag);
        }

        public void UpdateBookNodeTags(AiBookModel bookNode, string[] bookNodeTags)
        {
            if (bookNode == null)
                throw new ArgumentNullException(nameof(bookNode));

            //product tags
            var existingProductTags = GetAllBookNodeTagsByBookNodeId(bookNode.Id);
            var productTagsToRemove = new List<BookNodeTag>();
            foreach (var existingProductTag in existingProductTags)
            {
                var found = false;
                foreach (var newProductTag in bookNodeTags)
                {
                    if (!existingProductTag.Name.Equals(newProductTag, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    found = true;
                    break;
                }

                if (!found)
                {
                    productTagsToRemove.Add(existingProductTag);
                }
            }

            foreach (var productTag in productTagsToRemove)
            {

                bookNode.BookNodeBookNodeTagMappings
                    .Remove(bookNode.BookNodeBookNodeTagMappings.FirstOrDefault(mapping => mapping.BookNodeTagId == productTag.Id));
                //product.ProductTags.Remove(productTag);
                //  bookNode.n
                //     .Remove(product.ProductProductTagMappings.FirstOrDefault(mapping => mapping.ProductTagId == productTag.Id));
                //_productService.UpdateProduct(product);

                _aiBookService.UpdateAiBookModel(bookNode);
            }

            foreach (var productTagName in bookNodeTags)
            {
                BookNodeTag productTag;
                var productTag2 = GetBookNodeTagByName (productTagName);
                if (productTag2 == null)
                {
                    //add new product tag
                    productTag = new BookNodeTag
                    {
                        Name = productTagName
                    };
                    InsertBookNodeTag (productTag);
                }
                else
                {
                    productTag = productTag2;
                }

                if (!_aiBookService.BookNodeTagExists(bookNode, productTag.Id))
                {
                    //product.ProductTags.Add(productTag);
                    bookNode.BookNodeBookNodeTagMappings.Add(new BookNodeBookNodeTagMapping {  BookNodeTag = productTag });
                    _aiBookService.UpdateAiBookModel(bookNode);
                }

                var seName = _urlRecordService.ValidateSeName(productTag, string.Empty, productTag.Name, true);
                _urlRecordService.SaveSlug(productTag, seName, 0);
            }

            //cache
            _staticCacheManager.RemoveByPrefix(NopBookNodeDefault.BookNodesTagPrefixCacheKey);
        }


        #endregion
    }
}
