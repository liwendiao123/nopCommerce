using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.ModelBinders
{
    public interface IJsonPropertyMapper
    {
        Dictionary<string, Tuple<string, Type>> GetMap(Type type);
    }
}
