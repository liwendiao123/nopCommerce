using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nop.Core.Infrastructure;

namespace Nop.Core.LoginInfo
{
   public class AccountToken
    {
        public string ID { get; set; }
        public string ClientId { get; set; }
        public string UserName { get; set; }
        public long ExpireTime { get; set; }
        public string Serialize()
        {
            var sessionText = string.Format(@"{0}&{1}&{2}&{3}",
                this.ID,
                this.UserName,
                this.ClientId,
                this.ExpireTime );
            return Md5Util.EncryptDES(sessionText);
        }
        public static AccountToken Deserialize(string token)
        {
            if (string.IsNullOrEmpty( token))
                return null;
            var sessionText = Md5Util.DecryptDES(token);
            var tokens = sessionText.Split('&');
            if (tokens.Count() != 4)
                return null;
            return new AccountToken
            {
                        ID = tokens[0].ToString(),
              UserName   = tokens[1].ToString(),
              ClientId   = tokens[2],
              ExpireTime =Convert.ToInt64(string.IsNullOrEmpty(tokens[3])?
                                            DateTime.Now.AddDays(30).Ticks.ToString()
                                           : tokens[3].ToString())
            };
        }
    }
}
