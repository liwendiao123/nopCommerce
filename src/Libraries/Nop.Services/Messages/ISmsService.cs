using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.Messages.SMS;

namespace Nop.Services.Messages
{
   public partial interface ISmsService
    {
        QSResult<bool> SendMsg(SmsMsgRecord record);
        QSResult<bool> CheckMsgValid(SmsMsgRecord record);

        QSResult<bool> CheckMsgValidWithCode(SmsMsgRecord record);
        QSResult<bool> ApplySms(SmsMsgRecord record);

        
    }
}
