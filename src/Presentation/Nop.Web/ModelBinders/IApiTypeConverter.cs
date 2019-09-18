using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.ModelBinders
{
    public interface IApiTypeConverter
    {
        DateTime? ToUtcDateTimeNullable(string value);
        int ToInt(string value);
        int? ToIntNullable(string value);
        IList<int> ToListOfInts(string value);
        bool? ToStatus(string value);
        object ToEnumNullable(string value, Type type);
    }
}
