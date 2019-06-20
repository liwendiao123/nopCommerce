using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.TableOfContent;

namespace Nop.Services.TableOfContent
{

    /// <summary>
    /// 书籍目录管理服务
    /// </summary>
   public partial interface IBookDirService
    {
        /// <summary>
        /// Inserts a BookDir
        /// </summary>
        /// <param name="bookdir">bookdir</param>
        int InsertBookDir(BookDir bookdir);

        /// <summary>
        /// Updates the BookDir
        /// </summary>
        /// <param name="bookdir">BookDir</param>
        int UpdateBookDir(BookDir bookdir);


        /// <summary>
        /// Deletes a BookDir
        /// </summary>
        /// <param name="bookdir">bookdir</param>
        int DeleteBookDir(BookDir store);

        /// <summary>
        /// Gets all stores
        /// </summary>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Stores</returns>
        IList<BookDir> GetAllBookDirs(bool loadCacheableCopy = true);


        /// <summary>
        /// Gets a store 
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Store</returns>
        BookDir GetBookDirById(int storeId, bool loadCacheableCopy = true);


        /// <summary>
        /// Returns a list of names of not existing stores
        /// </summary>
        /// <param name="storeIdsNames">The names and/or IDs of the store to check</param>
        /// <returns>List of names and/or IDs not existing stores</returns>
        string[] GetNotExistingBookDirs(string[] storeIdsNames);
    }
}
