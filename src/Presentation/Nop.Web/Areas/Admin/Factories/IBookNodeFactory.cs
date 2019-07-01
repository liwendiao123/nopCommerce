using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Areas.Admin.Models.AiBook;

namespace Nop.Web.Areas.Admin.Factories
{
    public interface IBookNodeFactory
    {


        /// <summary>
        /// 获取课本添加、修改模型
        /// </summary>
        /// <param name="bookModelModel"></param>
        /// <param name="filterByBookId"></param>
        /// <returns></returns>
        AiBookModelView PrepareBookNodeModel(AiBookModelView bookModelModel, int? filterByBookId);

        /// <summary>
        ///  获取知识点搜索模型
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        AiBookSearchModelView PrepareBookNodeListModel(AiBookSearchModelView searchModel);
        AiBookSearchModelView PrepareBlogPostSearchModel(AiBookSearchModelView searchModel);
    }
}
