using Nop.Core.Domain.Customers;
using Nop.Core.Infrastructure;
using Nop.Plugin.SMS.Aliyun.Services;
using Nop.Services.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.SMS.Aliyun.Extensions
{
    public static class WorkflowMessageServiceExtensions
    {
        public static int SendCustomerRegisteredShortMessage(this IWorkflowMessageService workflowMessageService,  
            Customer customer, int languageId)
        {
            var shortMessageService = EngineContext.Current.Resolve<IShortMessageService>();
            shortMessageService.SendCustomerRegisteredShortMessage(customer, languageId);
            
            return 0;
        }

    }
}
