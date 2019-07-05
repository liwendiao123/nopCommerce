using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Stores;
using Nop.Plugin.SMS.Aliyun.Domain;
using Nop.Services.Localization;
using Nop.Services.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.SMS.Aliyun.Services
{
    public partial class SmsTemplateService: ISmsTemplateService
    {
        #region Constants

        /// <summary>
        /// Key for caching all records
        /// </summary>
        /// <remarks>
        /// {0} : page index
        /// {1} : page size
        /// </remarks>
        private const string SMSTEMPLATE_ALL_KEY = "Nop.smstemplate.all-{0}-{1}";
        private const string SMSTEMPLATE_PATTERN_KEY = "Nop.smstemplate.";

        #endregion

        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IRepository<SmsTemplate> _smsRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly ILanguageService _languageService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly ILocalizedEntityService _localizedEntityService;

        #endregion

        #region Ctor

        public SmsTemplateService(IRepository<SmsTemplate> sbwRepository,
            IRepository<StoreMapping> storeMappingRepository,
            ILanguageService languageService,
            ILocalizedEntityService localizedEntityService,
            IStoreMappingService storeMappingService,
            ICacheManager cacheManager)
        {
            this._smsRepository = sbwRepository;
            this._cacheManager = cacheManager;
            _languageService = languageService;
            _storeMappingRepository = storeMappingRepository;
            _storeMappingService = storeMappingService;
            _localizedEntityService = localizedEntityService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all shipping by weight records
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of the shipping by weight record</returns>
        public virtual IPagedList<SmsTemplate> GetAll(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var key = string.Format(SMSTEMPLATE_ALL_KEY, pageIndex, pageSize);
            return _cacheManager.Get(key, () =>
            {
                var query = from sbw in _smsRepository.Table
                                orderby sbw.TemplateCode descending
                            select sbw;

                var records = new PagedList<SmsTemplate>(query, pageIndex, pageSize);
                return records;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <returns></returns>
        public virtual SmsTemplate GetSmsTemplateBySystemName(string systemName, int storeId)
        {
            //var query = from s in _smsRepository.Table
            //            where s.SystemName == systemName
            //            //orderby s.SentOnUtc descending
            //            select s;

            var templates = GetAll().Where(t=> t.SystemName == systemName).ToList();

            //store mapping
            if (storeId > 0)
            {
                templates = templates
                    .Where(t => _storeMappingService.Authorize(t, storeId))
                    .ToList();
            }

            return templates.FirstOrDefault();
        }

        /// <summary>
        /// Get a shipping by weight record by identifier
        /// </summary>
        /// <param name="smsTemplateId">Record identifier</param>
        /// <returns>Shipping by weight record</returns>
        public virtual SmsTemplate GetSmsTemplateById(int smsTemplateId)
        {
            if (smsTemplateId == 0)
                return null;

            return _smsRepository.GetById(smsTemplateId);
        }

        /// <summary>
        /// Insert the shipping by weight record
        /// </summary>
        /// <param name="smsTemplate">Shipping by weight record</param>
        public virtual void InsertSmsTemplate(SmsTemplate smsTemplate)
        {
            if (smsTemplate == null)
                throw new ArgumentNullException(nameof(smsTemplate));

            _smsRepository.Insert(smsTemplate);

            _cacheManager.RemoveByPattern(SMSTEMPLATE_PATTERN_KEY);
        }

        /// <summary>
        /// Update the shipping by weight record
        /// </summary>
        /// <param name="smsTemplate">Shipping by weight record</param>
        public virtual void UpdateSmsTemplate(SmsTemplate smsTemplate)
        {
            if (smsTemplate == null)
                throw new ArgumentNullException(nameof(smsTemplate));

            _smsRepository.Update(smsTemplate);

            _cacheManager.RemoveByPattern(SMSTEMPLATE_PATTERN_KEY);
        }

        /// <summary>
        /// Delete the shipping by weight record
        /// </summary>
        /// <param name="smsTemplate">Shipping by weight record</param>
        public virtual void DeleteSmsTemplate(SmsTemplate smsTemplate)
        {
            if (smsTemplate == null)
                throw new ArgumentNullException(nameof(smsTemplate));

            _smsRepository.Delete(smsTemplate);

            _cacheManager.RemoveByPattern(SMSTEMPLATE_PATTERN_KEY);
        }

        public virtual SmsTemplate CopySmsTemplate(SmsTemplate smsTemplate)
        {
            if (smsTemplate == null)
                throw new ArgumentNullException(nameof(smsTemplate));

            var mtCopy = new SmsTemplate
            {
                SystemName = smsTemplate.SystemName,
                Subject = smsTemplate.Subject,
                Body = smsTemplate.Body,
                IsActive = smsTemplate.IsActive,
                
                LimitedToStores = smsTemplate.LimitedToStores,
                DelayBeforeSend = smsTemplate.DelayBeforeSend,
                DelayPeriod = smsTemplate.DelayPeriod
            };

            InsertSmsTemplate(mtCopy);

            var languages = _languageService.GetAllLanguages(true);

            //localization
            foreach (var lang in languages)
            {
                var subject = smsTemplate.GetLocalized(x => x.Subject, lang.Id, false, false);
                if (!string.IsNullOrEmpty(subject))
                    _localizedEntityService.SaveLocalizedValue(mtCopy, x => x.Subject, subject, lang.Id);

                var body = smsTemplate.GetLocalized(x => x.Body, lang.Id, false, false);
                if (!string.IsNullOrEmpty(body))
                    _localizedEntityService.SaveLocalizedValue(mtCopy, x => x.Body, body, lang.Id);

            }

            //store mapping
            var selectedStoreIds = _storeMappingService.GetStoresIdsWithAccess(smsTemplate);
            foreach (var id in selectedStoreIds)
            {
                _storeMappingService.InsertStoreMapping(mtCopy, id);
            }

            return mtCopy;
        }


        #endregion
    }
}
