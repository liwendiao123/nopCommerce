using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.Media;

namespace Nop.Core.Domain.Customers
{
  public  class CustomerPicture
    {
        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets the picture
        /// </summary>
        public virtual Picture Picture { get; set; }

        /// <summary>
        /// Gets the product
        /// </summary>
        public virtual Customer Product { get; set; }
    }
}
