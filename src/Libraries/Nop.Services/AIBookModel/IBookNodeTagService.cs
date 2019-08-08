using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.AIBookModel;

namespace Nop.Services.AIBookModel
{
  public  partial  interface IBookNodeTagService
    {

        /// <summary>
        /// 删除指定知识点 关键字
        /// </summary>
        /// <param name="bookNodeTag">只是点标签</param>
        void DeleteBookNodeTag(BookNodeTag bookNodeTag);

        /// <summary>
        /// 获取所有知识点的关键字
        /// </summary>
        /// <returns></returns>
        IList<BookNodeTag> GetAllBookNodeTags();

        /// <summary>
        /// 获取指定知识点的所有关键字
        /// </summary>
        /// <param name="BookNodeId">知识点ID</param>
        /// <returns></returns>
        IList<BookNodeTag> GetAllBookNodeTagsByBookNodeId(int bookNodeId);

        /// <summary>
        /// 获取指定知识点标签
        /// </summary>
        /// <param name="booknodeTagId">知识点标签ID</param>
        /// <returns></returns>
        BookNodeTag GetBookNodeTagById(int booknodeTagId);

        /// <summary>
        /// 根据关键字 自定信息 获取标签信息
        /// </summary>
        /// <param name="name">知识点标签名称</param>
        /// <returns></returns>
        BookNodeTag GetBookNodeTagByName(string name);

        /// <summary>
        /// 添加指定知识点关键字
        /// </summary>
        /// <param name="bookNodeTag"></param>
        void InsertBookNodeTag(BookNodeTag bookNodeTag);

        /// <summary>
        /// 更新指定标签
        /// </summary>
        /// <param name="bookNodeTag">知识点标签</param>
        void UpdateBookNodeTag(BookNodeTag bookNodeTag);

        /// <summary>
        /// 获取指定标签关联的 知识点数量
        /// </summary>
        /// <param name="bookNodeTagId">知识点ID</param>
        /// <param name="storeId">门店ID</param>
        /// <param name="showHidden">展示或隐藏</param>
        /// <returns></returns>
        int GetBookNodeCount(int bookNodeTagId, int storeId, bool showHidden = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookNode">要更新的知识点</param>
        /// <param name="bookNodeTags">知识点标签</param>
        void UpdateBookNodeTags(AiBookModel bookNode, string[] bookNodeTags);

        IList<BookNodeTag> GetAllBookNodeTagsByBookNodeBySearchName(string bookname);

    }
}
