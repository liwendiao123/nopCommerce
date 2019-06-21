using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.AiBook
{

    /// <summary>
    /// 
    /// </summary>
    public partial class AiBookModelView : BaseNopEntityModel, ILocalizedModel<AiBookModelLocalizedModel>
    {

        #region  Field
        public IList<AiBookModelLocalizedModel> Locales { get; set; }
        #endregion

        #region Ctor 构造器
        public AiBookModelView()
        {
            Locales = new List<AiBookModelLocalizedModel>();           
        }
        #endregion


        #region 属性


      // public string 

        #endregion
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class AiBookModelLocalizedModel : ILocalizedModel
    {

        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.AiBook.AiBookModel.Fields.Name")]
        public string Name { get; set; }
    }
}
