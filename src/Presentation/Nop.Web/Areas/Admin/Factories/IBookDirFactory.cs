using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Areas.Admin.Models.TableOfContent;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial interface IBookDirFactory
    {
        BookDirSearchModel PrepareBookDirSearchModel(BookDirSearchModel searchModel, BookDirModel bdm);

        BookDirListModel PrepareBookDirListModel();

        BookDirModel PrepareBookDirModel();
    }
}
