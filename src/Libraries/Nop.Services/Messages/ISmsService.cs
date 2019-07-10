using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.Messages.SMS;

namespace Nop.Services.Messages
{
   public partial interface ISmsService
    {

        bool SendMsg(SmsMsgRecord record);

        bool CheckMsgValid(SmsMsgRecord record);

        bool ApplySms(SmsMsgRecord record);

        
    }
}
