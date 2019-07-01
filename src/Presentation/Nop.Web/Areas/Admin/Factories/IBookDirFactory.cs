using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core.Domain.TableOfContent;
using Nop.Web.Areas.Admin.Models.TableOfContent;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial interface IBookDirFactory
    {
        BookDirSearchModel PrepareBookDirSearchModel(BookDirSearchModel searchModel, BookDirModel bdm);

        BookDirListModel PrepareBookDirListModel(BookDirSearchModel model);
    
        BookDirModel PrepareBookDirModel(BookDirModel bookdirModel,BookDir entity, bool excludeProperties = false);
        BookDirModel PrepareBookDirModel();
    }
}
