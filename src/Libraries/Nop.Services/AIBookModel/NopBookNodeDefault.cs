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
        public static string BookNodeMappingByEntityIdNameCacheKey => "Nop.bookNodemapping.entityid-name-{0}-{1}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string BookNodeMappingPrefixCacheKey => "Nop.bookNodemapping.";

        #endregion

        #region BookDir

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string BookNodesAllCacheKey => "Nop.BookNode.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        public static string BookNodesByIdCacheKey => "Nop.BookNode.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string BookNodesPrefixCacheKey => "Nop.BookNodes.";

        public static string GetChildBookNodesByParentIdCacheKey => "Nop.BookNode.SubDir.ids-{0}";

        #endregion
    }
}
