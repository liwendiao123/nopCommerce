using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.AIBookModel;
using Nop.Core.Domain.TableOfContent;

namespace Nop.Core.Domain.Customers
{
    public class CustomerBookNodeLog : BaseEntity
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 书籍目录ID
        /// </summary>
        public int BookNodeId { get; set; }
        /// <summary>
        /// 学习时长
        /// </summary>
        public long LearnTime { get; set; }
        /// <summary>
        /// 是否 
        /// </summary>
        public bool IsRead { get; set; }


        public virtual Customer Customer { get; set; }


        public virtual AiBookModel BookNode { get; set; }


        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
