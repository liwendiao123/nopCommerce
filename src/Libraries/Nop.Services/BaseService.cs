using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Infrastructure;

namespace Nop.Services
{
   public class BaseService
    {
        protected QSResult<T> Result<T>(T model, ErrorCode code = ErrorCode.sys_success)
        {
            return new QSResult<T>
            {
                Code = code,
                Result = model
            };
        }

        protected QSResult<T> Result<T>(T model, string msg, ErrorCode code = ErrorCode.sys_success)
        {
            return new QSResult<T>
            {
                Code = code,
                Result = model,
                Msg = msg
            };
        }
    }
}
