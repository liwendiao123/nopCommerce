
using Nop.Plugin.SMS.Aliyun.Domain;
using Nop.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.SMS.Aliyun.Extensions
{
    public static class EventPublisherExtensions
    {
        public static void MessageTokensAdded<U>(this IEventPublisher eventPublisher, SmsTemplate message, System.Collections.Generic.IList<U> tokens)
        {
            eventPublisher.Publish(new MessageTokensAddedEvent<U>(message, tokens));
        }
    }
}
