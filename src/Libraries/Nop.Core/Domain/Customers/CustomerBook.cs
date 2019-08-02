using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.AIBookModel;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.TableOfContent;

namespace Nop.Core.Domain.Customers
{
    public class CustomerBook : BaseEntity
    {


        public int CustomerId { get; set; }
        /// <summary>
        /// 课本id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 知识点ID
        /// </summary>
        public int BookNodeId { get; set; }


        /// <summary>
        /// 目录ID
        /// </summary>
        public int BookBookDirId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 类别标识
        /// </summary>
        public int TypeLabel { get; set; }
        /// <summary>
        /// 客户信息
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// 书籍目录信息
        /// </summary>
        public virtual BookDir BookDir { get; set; }
        /// <summary>
        /// 书籍节点信息
        /// </summary>
        public virtual AiBookModel BookNode { get; set; }   
        /// <summary>
        /// 书籍
        /// </summary>
        public virtual Product Product { get; set; }
        public DateTime Expirationtime { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
             
    }
}
