using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Configuration;

namespace Nop.Plugin.Sms.AliyunSMS
{
   public class AliSendSmsSetting: ISettings
    {
       
        public string RegionId { get; set; }
        public string AliSmsID => "LTAIlhLpMHU1yRq5";

        public string AliSmsSecret => "etG9x3mYQSUPwqd3emaLnDoiOqdjWv";
    }
}
