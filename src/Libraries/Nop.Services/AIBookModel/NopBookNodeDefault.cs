using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Services.AIBookModel
{
  public  class NopBookNodeDefault
    {
        #region Store mappings

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity name
        /// </remarks>
        public static string BookNodeMappingByEntityIdNameCacheKey => "Nop.BookNodes.mapping.entityid-name-{0}-{1}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string BookNodeMappingPrefixCacheKey => "Nop.BookNodes.mapping.";

        #endregion

        #region BookDir

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string BookNodesAllCacheKey => "Nop.BookNodes.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        public static string BookNodesByIdCacheKey => "Nop.BookNodes.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string BookNodesPrefixCacheKey => "Nop.BookNodes.";

        public static string GetChildBookNodesByParentIdCacheKey => "Nop.BookNodes.SubDir.ids-{0}";


        public static string BookNodesByBookDirIdsCacheKey => "Nop.BookNodes.BookDirIds-{0}";

        #endregion
    }
}
