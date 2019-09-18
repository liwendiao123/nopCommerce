using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.ModelBinders
{
    public interface IObjectConverter
    {
        T ToObject<T>(ICollection<KeyValuePair<string, string>> source)
            where T : class, new();
    }
}
