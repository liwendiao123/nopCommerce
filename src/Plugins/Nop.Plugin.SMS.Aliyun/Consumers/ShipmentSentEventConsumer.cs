using Nop.Core.Domain.Shipping;
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

    public partial class ShipmentSentEventConsumer : IConsumer<ShipmentSentEvent>
    {

        #region Fields

        private readonly ISettingService _settingService;
        private readonly IShortMessageService _workflowMessageService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public ShipmentSentEventConsumer(ISettingService settingService,
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
        public void HandleEvent(ShipmentSentEvent eventMessage)
        {
            //_workflowMessageService.SendCustomerRegisteredShortMessage(eventMessage.Customer, _workContext.WorkingLanguage.Id);
        }


        #endregion
    }
}
