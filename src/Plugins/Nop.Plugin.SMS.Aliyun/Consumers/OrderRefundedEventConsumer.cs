using Nop.Core.Domain.Orders;
using Nop.Services.Configuration;
using Nop.Services.Events;
using Nop.Services.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.SMS.Aliyun.Services;

namespace Nop.Plugin.SMS.Aliyun.Consumers
{

    public partial class OrderRefundedEventConsumer : IConsumer<OrderRefundedEvent>
    {

        #region Fields

        private readonly ISettingService _settingService;
        private readonly IShortMessageService _workflowMessageService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public OrderRefundedEventConsumer(ISettingService settingService,
            IShortMessageService workflowMessageService,
            IWorkContext workContext)
        {
            this._settingService = settingService;
            this._workflowMessageService = workflowMessageService;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handle shipping method deleted event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        public void HandleEvent(OrderRefundedEvent eventMessage)
        {
            //_workflowMessageService.SendCustomerRegisteredShortMessage(eventMessage.Customer, _workContext.WorkingLanguage.Id);
        }


        #endregion
    }
}
