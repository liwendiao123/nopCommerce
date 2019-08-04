using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core;
using Nop.Core.Domain.AIBookModel;
using Nop.Core.Domain.Catalog;

namespace Nop.Services.AIBookModel
{
   public interface IAiBookService
    {
        /// <summary>
        /// Inserts a BookDir
        /// </summary>
        /// <param name="bookdir">bookdir</param>
        int InsertAiBookModel(AiBookModel bookdir);

        /// <summary>
        /// Updates the BookDir
        /// </summary>
        /// <param name="bookdir">BookDir</param>
        int UpdateAiBookModel(AiBookModel bookdir);


        /// <summary>
        /// Deletes a BookDir
        /// </summary>
        /// <param name="bookdir">bookdir</param>
        int DeleteAiBookModel(AiBookModel store);



        /// <summary>
        /// Gets all stores
        /// </summary>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Stores</returns>
        IList<AiBookModel> GetAllAiBookModels(bool loadCacheableCopy = true);


        /// <summary>
        /// Gets a store 
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Store</returns>
        AiBookModel GetAiBookModelById(int storeId, bool loadCacheableCopy = true);
        bool BookNodeTagExists(AiBookModel product, int productTagId);

        /// <summary>
        ///根据 AR识别图名称 获取知识点
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Store</returns>
        AiBookModel GetAiBookModelByArImgName(string imgName, bool loadCacheableCopy = true);


        IList<AiBookModel> AiBookModelByBookDirLastNodeId(List<int> bookdirIds, bool loadCacheableCopy = true);

        /// <summary>
        /// Returns a list of names of not existing stores
        /// </summary>
        /// <param name="storeIdsNames">The names and/or IDs of the store to check</param>
        /// <returns>List of names and/or IDs not existing stores</returns>
        string[] GetNotExistingAiBookModels(string[] storeIdsNames);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aibookNodeName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryIds"></param>
        /// <param name="bookId"></param>
        /// <param name="bookdirId"></param>
        /// <param name="vendorId"></param>
        /// <param name="visibleIndividuallyOnly"></param>
        /// <param name="keywords"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        IPagedList<AiBookModel> SearchAiBookModels(string aibookNodeName, int pageIndex = 0, int pageSize = int.MaxValue, IList<int> categoryIds = null, int bookId = 0, int bookdirId = 0, int vendorId = 0, bool visibleIndividuallyOnly = false, string keywords = null, bool showHidden = false);

        #region News comments

        /// <summary>
        /// Gets all comments
        /// </summary>
        /// <param name="customerId">Customer identifier; 0 to load all records</param>
        /// <param name="storeId">Store identifier; pass 0 to load all records</param>
        /// <param name="newsItemId">News item ID; 0 or null to load all records</param>
        /// <param name="approved">A value indicating whether to content is approved; null to load all records</param> 
        /// <param name="fromUtc">Item creation from; null to load all records</param>
        /// <param name="toUtc">Item creation to; null to load all records</param>
        /// <param name="commentText">Search comment text; null to load all records</param>
        /// <returns>Comments</returns>
        IList<BookNodeComment> GetAllComments(int customerId = 0, int storeId = 0, int? newsItemId = null,
            bool? approved = null, DateTime? fromUtc = null, DateTime? toUtc = null, string commentText = null);

        /// <summary>
        /// Gets a news comment
        /// </summary>
        /// <param name="newsCommentId">News comment identifier</param>
        /// <returns>News comment</returns>
        BookNodeComment GetNewsCommentById(int newsCommentId);

        /// <summary>
        /// Get news comments by identifiers
        /// </summary>
        /// <param name="commentIds">News comment identifiers</param>
        /// <returns>News comments</returns>
        IList<BookNodeComment> GetNewsCommentsByIds(int[] commentIds);

        /// <summary>
        /// Get the count of news comments
        /// </summary>
        /// <param name="newsItem">News item</param>
        /// <param name="storeId">Store identifier; pass 0 to load all records</param>
        /// <param name="isApproved">A value indicating whether to count only approved or not approved comments; pass null to get number of all comments</param>
        /// <returns>Number of news comments</returns>
        int GetNewsCommentsCount(BookNodeComment newsItem, int storeId = 0, bool? isApproved = null);

        /// <summary>
        /// Deletes a news comment
        /// </summary>
        /// <param name="newsComment">News comment</param>
        void DeleteNewsComment(BookNodeComment newsComment);

        /// <summary>
        /// Deletes a news comments
        /// </summary>
        /// <param name="newsComments">News comments</param>
        void DeleteNewsComments(IList<BookNodeComment> newsComments);

        #endregion
    }


}
