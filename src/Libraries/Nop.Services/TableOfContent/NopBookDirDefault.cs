using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Services.TableOfContent
{
  public class NopBookDirDefault
    {
        #region Store mappings

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity name
        /// </remarks>
        public static string BookDirMappingByEntityIdNameCacheKey => "Nop.bookDirmapping.entityid-name-{0}-{1}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string BookDirMappingPrefixCacheKey => "Nop.bookDirmapping.";

        #endregion

        #region BookDir

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string BookDirsAllCacheKey => "Nop.BookDir.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        public static string BookDirsByIdCacheKey => "Nop.BookDir.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string BookDirsPrefixCacheKey => "Nop.BookDir.";

        public static string GetChildBookDirsByParentIdCacheKey => "Nop.BookDir.SubDir.ids-{0}";
        public static string GetChildBookDirItemsByParentIdCacheKey => "Nop.BookDir.SubDir.items-{0}";
        #endregion
    }
}
