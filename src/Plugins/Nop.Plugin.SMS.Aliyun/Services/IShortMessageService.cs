using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Plugin.SMS.Aliyun.Domain;
using Nop.Services.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.SMS.Aliyun.Services
{
    public partial interface IShortMessageService
    {
        SendShortMessageResult SendCustomerRegisteredShortMessage(Customer customer, int languageId);
        //SendShortMessageResult SendTestShortMessage(int messageTemplateId, string sendTo, List<Token> tokens, int languageId);

        SendShortMessageResult SendTestShortMessage(SmsTemplate template, string sendTo, int languageId);

        #region Order workflow

        /// <summary>
        /// Sends an order paid notification to a store owner
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        SendShortMessageResult SendOrderPaidStoreOwnerNotification(Order order, int languageId);
        /// <summary>
        /// Sends a shipment sent notification to a customer
        /// </summary>
        /// <param name="shipment">Shipment</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        SendShortMessageResult SendShipmentSentCustomerNotification(Shipment shipment, int languageId);

        /// <summary>
        /// Sends a shipment delivered notification to a customer
        /// </summary>
        /// <param name="shipment">Shipment</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        SendShortMessageResult SendShipmentDeliveredCustomerNotification(Shipment shipment, int languageId);

        /// <summary>
        /// Sends 'New Return Request' message to a customer
        /// </summary>
        /// <param name="returnRequest">Return request</param>
        /// <param name="orderItem">Order item</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        SendShortMessageResult SendNewReturnRequestCustomerNotification(ReturnRequest returnRequest, OrderItem orderItem, int languageId);

        #endregion

        #region Order workflow

        #endregion

        #region Order workflow

        #endregion
    }
}
