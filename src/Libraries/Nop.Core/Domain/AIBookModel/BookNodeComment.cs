using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.News;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Stores;

namespace Nop.Core.Domain.AIBookModel
{
   public  class BookNodeComment: BaseEntity
    {
        /// <summary>
        /// Gets or sets the comment title
        /// </summary>
        public string CommentTitle { get; set; }
        /// <summary>
        /// Gets or sets the comment text
        /// </summary>
        public string CommentText { get; set; }
        /// <summary>
        /// Gets or sets the news item identifier
        /// </summary>
        public int BookNodeId { get; set; }
        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the comment is approved
        /// </summary>
        public bool IsApproved { get; set; }
        /// <summary>
        /// Gets or sets the store identifier
        /// </summary>
        public int StoreId { get; set; }
        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }
        /// <summary>
        /// Gets or sets the customer
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// Gets or sets the news item
        /// </summary>
        public virtual AiBookModel BookNodeItem { get; set; }
        /// <summary>
        /// Gets or sets the store
        /// </summary>
        public virtual Store Store { get; set; }


    }
}
