using Nop.Core;
using Nop.Plugin.SMS.Aliyun.Domain;

namespace Nop.Plugin.SMS.Aliyun.Services
{
    /// <summary>
    /// Represents service shipping by weight service
    /// </summary>
    public partial interface IQueuedSmsService
    {
        /// <summary>
        /// Get all shipping by weight records
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of the shipping by weight record</returns>
        IPagedList<QueuedSms> GetAll(int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <returns></returns>
        QueuedSms GetQueuedSms(string mobileNumber);

        /// <summary>
        /// Get a shipping by weight record by identifier
        /// </summary>
        /// <param name="queuedSmsId">Record identifier</param>
        /// <returns>Shipping by weight record</returns>
        QueuedSms GetById(int queuedSmsId);

        /// <summary>
        /// Insert the shipping by weight record
        /// </summary>
        /// <param name="queuedSms">Shipping by weight record</param>
        void InsertQueuedSms(QueuedSms queuedSms);

        /// <summary>
        /// Update the shipping by weight record
        /// </summary>
        /// <param name="queuedSms">Shipping by weight record</param>
        void UpdateQueuedSms(QueuedSms queuedSms);

        /// <summary>
        /// Delete the shipping by weight record
        /// </summary>
        /// <param name="queuedSms">Shipping by weight record</param>
        void DeleteQueuedSms(QueuedSms queuedSms);
    }
}
