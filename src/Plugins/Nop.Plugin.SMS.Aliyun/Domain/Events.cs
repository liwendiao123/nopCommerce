using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.SMS.Aliyun.Domain
{
    public class MessageTokensAddedEvent<U>
    {
        private readonly SmsTemplate _message;
        private readonly IList<U> _tokens;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="tokens">Tokens</param>
        public MessageTokensAddedEvent(SmsTemplate message, IList<U> tokens)
        {
            _message = message;
            _tokens = tokens;
        }

        /// <summary>
        /// Message
        /// </summary>
        public SmsTemplate Message { get { return _message; } }
        /// <summary>
        /// Tokens
        /// </summary>
        public IList<U> Tokens { get { return _tokens; } }
    }
}
