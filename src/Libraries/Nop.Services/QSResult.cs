using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Infrastructure;
namespace Nop.Services
{

    
    public class QSResult<T>
    {
        public ErrorCode Code;

        public string Msg { get; set; }

        public T Result { get; set; }
    }
}
