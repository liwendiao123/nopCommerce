using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Plugin.SMS.Aliyun.Domain;
using System;
using System.Linq;

namespace Nop.Plugin.SMS.Aliyun.Services
{
    /// <summary>
    /// Represents service shipping by weight service implementation
    /// </summary>
    public partial class QueuedSmsService : IQueuedSmsService
    {
        #region Constants

        /// <summary>
        /// Key for caching all records
        /// </summary>
        /// <remarks>
        /// {0} : page index
        /// {1} : page size
        /// </remarks>
        private const string QUEUEDSMS_ALL_KEY = "Nop.queuedsms.all-{0}-{1}";
        private const string QUEUEDSMS_PATTERN_KEY = "Nop.queuedsms.";

        #endregion

        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IRepository<QueuedSms> _smsRepository;

        #endregion

        #region Ctor

        public QueuedSmsService(IRepository<QueuedSms> sbwRepository,
            ICacheManager cacheManager)
        {
            this._smsRepository = sbwRepository;
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all shipping by weight records
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of the shipping by weight record</returns>
        public virtual IPagedList<QueuedSms> GetAll(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var key = string.Format(QUEUEDSMS_ALL_KEY, pageIndex, pageSize);
            return _cacheManager.Get(key, () =>
            {
                var query = from s in _smsRepository.Table
                            orderby  s.CreatedOnUtc descending
                            select s;

                var records = new PagedList<QueuedSms>(query, pageIndex, pageSize);
                return records;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <returns></returns>
        public virtual QueuedSms GetQueuedSms( string mobileNumber)
        {
            var query = from s in _smsRepository.Table
                        where s.ToMobileNumber == mobileNumber
                        orderby s.SentOnUtc descending
                        select s;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Get a shipping by weight record by identifier
        /// </summary>
        /// <param name="queuedSmsId">Record identifier</param>
        /// <returns>Shipping by weight record</returns>
        public virtual QueuedSms GetById(int queuedSmsId)
        {
            if (queuedSmsId == 0)
                return null;

            return _smsRepository.GetById(queuedSmsId);
        }

        /// <summary>
        /// Insert the shipping by weight record
        /// </summary>
        /// <param name="queuedSms">Shipping by weight record</param>
        public virtual void InsertQueuedSms(QueuedSms queuedSms)
        {
            if (queuedSms == null)
                throw new ArgumentNullException(nameof(queuedSms));

            _smsRepository.Insert(queuedSms);

            _cacheManager.RemoveByPattern(QUEUEDSMS_PATTERN_KEY);
        }

        /// <summary>
        /// Update the shipping by weight record
        /// </summary>
        /// <param name="queuedSms">Shipping by weight record</param>
        public virtual void UpdateQueuedSms(QueuedSms queuedSms)
        {
            if (queuedSms == null)
                throw new ArgumentNullException(nameof(queuedSms));

            _smsRepository.Update(queuedSms);

            _cacheManager.RemoveByPattern(QUEUEDSMS_PATTERN_KEY);
        }

        /// <summary>
        /// Delete the shipping by weight record
        /// </summary>
        /// <param name="queuedSms">Shipping by weight record</param>
        public virtual void DeleteQueuedSms(QueuedSms queuedSms)
        {
            if (queuedSms == null)
                throw new ArgumentNullException(nameof(queuedSms));

            _smsRepository.Delete(queuedSms);

            _cacheManager.RemoveByPattern(QUEUEDSMS_PATTERN_KEY);
        }

        #endregion
    }
}
