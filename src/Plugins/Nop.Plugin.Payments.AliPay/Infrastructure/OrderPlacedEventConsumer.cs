using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.Orders;
using Nop.Services.Events;

namespace Nop.Plugin.Payments.AliPay.Infrastructure
{
    public class OrderPlacedEventConsumer : IConsumer<OrderPlacedEvent>
    {
        #region Fields

        private readonly IPluginFinder _pluginFinder;

        #endregion

        #region Ctor

        public OrderPlacedEventConsumer(IPluginFinder pluginFinder)
        {
            this._pluginFinder = pluginFinder;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="eventMessage">The event message.</param>
        public void HandleEvent(OrderPlacedEvent eventMessage)
        {
            //check that plugin is installed
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("Mobile.SMS.Clickatell");

            var plugin = pluginDescriptor?.Instance() as ClickatellSmsProvider;

            plugin?.SendSms(string.Empty, eventMessage.Order.Id);
        }

        #endregion
    }
}
