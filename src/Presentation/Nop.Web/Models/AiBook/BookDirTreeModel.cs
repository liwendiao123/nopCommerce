using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.AiBook
{
    public class BookDirTreeModel
    {

        public BookDirTreeModel()
        {

            Children = new
                 List<BookDirTreeModel>();
        }


        /// <summary>
        /// //章节ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// //上级ID
        /// </summary>
        public int PId { get; set; }

        /// <summary>
        /// 是否已经购买 解锁
        /// </summary>
        public bool IsLock { get; set; }

        /// <summary>
        /// 所属课本ID
        /// </summary>
        public int BookID { get; set; } 


        public bool IsRead { get; set; }

        /// <summary>
        /// 章节名称
        /// </summary>
        public string Name { get; set; }//
        /// <summary>
        /// 章节描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 价格描述  如果为零 则免费 否则展示需要付费的价格
        /// </summary>
        public string PriceRanges { get; set; }
        /// <summary>
        /// 展示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 是否为知识点
        /// </summary>
        public bool IsLastNode { get; set; }  

        /// <summary>
        /// 收费费复杂知识点
        /// </summary>
        public int ComplexLevel { get; set; }

        /// <summary>
        /// 展示图片= "http://arbookresouce.73data.cn/book/img/sy_img_02.png",//封面展示
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// //获取对应知识点 Url  
        /// </summary>
        public string BookNodeUrl { get; set; } 


        public List<BookDirTreeModel> Children { get; set; }
    }
}
