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
    }
}
