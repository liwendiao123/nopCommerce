using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Seo;

namespace Nop.Core.Domain.AIBookModel
{
   public class BookNodeTag: BaseEntity, ILocalizedEntity, ISlugSupported
    {
          private ICollection<BookNodeBookNodeTagMapping> _bookNodeBookNodeTagMappings;
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets product-product tag mappings
        /// </summary>
        public virtual ICollection<BookNodeBookNodeTagMapping> ProductProductTagMappings
        {
            get => _bookNodeBookNodeTagMappings ?? (_bookNodeBookNodeTagMappings = new List<BookNodeBookNodeTagMapping>());
            protected set => _bookNodeBookNodeTagMappings = value;
        }
    }
}
