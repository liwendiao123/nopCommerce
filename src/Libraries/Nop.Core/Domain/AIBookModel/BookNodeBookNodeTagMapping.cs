using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Core.Domain.AIBookModel
{
    public class BookNodeBookNodeTagMapping : BaseEntity
    {
        /// <summary>
        /// Gets or sets the BookNode identifier
        /// </summary>
        public int BookNodeId { get; set; }

        /// <summary>
        /// Gets or sets the BookNode tag identifier
        /// </summary>
        public int BookNodeTagId { get; set; }

     
        public virtual AiBookModel BookNode { get; set; }

        public virtual BookNodeTag BookNodeTag { get; set; }
    }
}
