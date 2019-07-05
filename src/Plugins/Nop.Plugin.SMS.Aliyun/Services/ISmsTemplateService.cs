using Nop.Core;
using Nop.Plugin.SMS.Aliyun.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.SMS.Aliyun.Services
{
    public partial interface ISmsTemplateService
    {
       
        /// <summary>
        /// Get all shipping by weight records
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of the shipping by weight record</returns>
        IPagedList<SmsTemplate> GetAll(int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Get a shipping by weight record by passed parameters
        /// </summary>
        /// <param name="shippingMethodId">Shipping method identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="warehouseId">Warehouse identifier</param>
        /// <param name="countryId">Country identifier</param>
        /// <param name="stateProvinceId">State identifier</param>
        /// <param name="zip">Zip postal code</param>
        /// <param name="weight">Weight</param>
        /// <returns>Shipping by weight record</returns>
        SmsTemplate GetSmsTemplateBySystemName(string systemName, int storeId);

        /// <summary>
        /// Get a shipping by weight record by identifier
        /// </summary>
        /// <param name="smsTemplateId">Record identifier</param>
        /// <returns>Shipping by weight record</returns>
        SmsTemplate GetSmsTemplateById(int smsTemplateId);

        /// <summary>
        /// Insert the shipping by weight record
        /// </summary>
        /// <param name="smsTemplate">Shipping by weight record</param>
        void InsertSmsTemplate(SmsTemplate smsTemplate);

        /// <summary>
        /// Update the shipping by weight record
        /// </summary>
        /// <param name="smsTemplate">Shipping by weight record</param>
        void UpdateSmsTemplate(SmsTemplate smsTemplate);

        /// <summary>
        /// Delete the shipping by weight record
        /// </summary>
        /// <param name="smsTemplate">Shipping by weight record</param>
        void DeleteSmsTemplate(SmsTemplate smsTemplate);

        SmsTemplate CopySmsTemplate(SmsTemplate smsTemplate);

    }
}
