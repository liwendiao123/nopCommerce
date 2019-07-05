using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.Models;

namespace Nop.Plugin.SMS.Aliyun.Models
{
    public partial class SmsTemplateModel : BaseNopEntityModel, ILocalizedModel<SmsTemplateLocalizedModel>
    {
        
        public SmsTemplateModel()
        {
            Locales = new List<SmsTemplateLocalizedModel>();
            //AvailableEmailAccounts = new List<SelectListItem>();

            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();
        }


        [NopResourceDisplayName("Plugins.SMS.Aliyun.SmsTemplate.Fields.SystemName")]
        public string SystemName { get; set; }
        [NopResourceDisplayName("Plugins.SMS.Aliyun.SmsTemplate.Fields.TemplateCode")]
        public string TemplateCode { get; set; }


        [NopResourceDisplayName("Plugins.SMS.Aliyun.SmsTemplate.Fields.Subject")]
        public string Subject { get; set; }

        [NopResourceDisplayName("Plugins.SMS.Aliyun.SmsTemplate.Fields.Body")]
        public string Body { get; set; }

        [NopResourceDisplayName("Plugins.SMS.Aliyun.SmsTemplate.Fields.IsActive")]
        public bool IsActive { get; set; }

        [NopResourceDisplayName("Plugins.SMS.Aliyun.SmsTemplate.Fields.SendImmediately")]
        public bool SendImmediately { get; set; }

        [NopResourceDisplayName("Plugins.SMS.Aliyun.SmsTemplate.Fields.DelayBeforeSend")]
        [UIHint("Int32Nullable")]
        public int? DelayBeforeSend { get; set; }
        public int DelayPeriodId { get; set; }

        //store mapping
        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.LimitedToStores")]
        public IList<int> SelectedStoreIds { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
        //comma-separated list of stores used on the list page
        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.LimitedToStores")]
        public string ListOfStores { get; set; }

        public IList<SmsTemplateLocalizedModel> Locales { get; set; }

        [NopResourceDisplayName("Plugins.SMS.Aliyun.QueuedSms.Fields.ToMobileNumber")]
        public string ToMobileNumber { get; set; }

        [NopResourceDisplayName("Plugins.SMS.Aliyun.QueuedSms.Fields.ToMobileNumber")]
        public string VariableValue { get; set; }
    }

    public partial class SmsTemplateLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Plugins.SMS.Aliyun.SmsTemplate.Fields.Subject")]
        public string Subject { get; set; }

        [NopResourceDisplayName("Plugins.SMS.Aliyun.SmsTemplate.Fields.Body")]
        public string Body { get; set; }

        
    }

}