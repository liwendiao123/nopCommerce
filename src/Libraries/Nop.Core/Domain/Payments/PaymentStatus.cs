namespace Nop.Core.Domain.Payments
{
    /// <summary>
    /// Represents a payment status enumeration
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// Pending //未付款
        /// </summary>
        Pending = 10,

        /// <summary>
        /// Authorized
        /// </summary>
        Authorized = 20,

        /// <summary>
        /// Paid  已支付
        /// </summary>
        Paid = 30,

        /// <summary>
        /// Partially Refunded //部分退款
        /// </summary>
        PartiallyRefunded = 35,

        /// <summary>
        /// Refunded ///已退款
        /// </summary>
        Refunded = 40,

        /// <summary>
        /// Voided //  废弃
        /// </summary>
        Voided = 50
    }
}
