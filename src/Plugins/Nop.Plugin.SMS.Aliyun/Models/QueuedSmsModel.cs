using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.Models;

namespace Nop.Plugin.SMS.Aliyun.Models
{
    public partial class QueuedSmsModel : BaseNopEntityModel
    {
        public QueuedSmsModel()
        {
            AvailableCountries = new List<SelectListItem>();
            AvailableStates = new List<SelectListItem>();
            AvailableShippingMethods = new List<SelectListItem>();
            AvailableStores = new List<SelectListItem>();
            AvailableWarehouses = new List<SelectListItem>();
        }



        [NopResourceDisplayName("Plugins.SMS.Aliyun.QueuedSms.Fields.Priority")]
        public string PriorityName { get; set; }
        [NopResourceDisplayName("Plugins.SMS.Aliyun.QueuedSms.Fields.ToMobileNumber")]
        public string ToMobileNumber { get; set; }
        [NopResourceDisplayName("Plugins.SMS.Aliyun.QueuedSms.Fields.ToName")]
        public string ToName { get; set; }

        [NopResourceDisplayName("Plugins.SMS.Aliyun.QueuedSms.Fields.Subject")]
        public string Subject { get; set; }

        [NopResourceDisplayName("Plugins.SMS.Aliyun.QueuedSms.Fields.TemplateCode")]
        public string TemplateCode { get; set; }

        [NopResourceDisplayName("Plugins.SMS.Aliyun.QueuedSms.Fields.TemplateParamJson")]
        public string TemplateParamJson { get; set; }


        [NopResourceDisplayName("Plugins.SMS.Aliyun.QueuedSms.Fields.Body")]
        public string Body { get; set; }


        [NopResourceDisplayName("Plugins.SMS.Aliyun.QueuedSms.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [NopResourceDisplayName("Plugins.SMS.Aliyun.QueuedSms.Fields.SendImmediately")]
        public bool SendImmediately { get; set; }

        [NopResourceDisplayName("Plugins.SMS.Aliyun.QueuedSms.Fields.DontSendBeforeDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? DontSendBeforeDate { get; set; }

        [NopResourceDisplayName("Plugins.SMS.Aliyun.QueuedSms.Fields.SentTries")]
        public int SentTries { get; set; }

        [NopResourceDisplayName("Plugins.SMS.Aliyun.QueuedSms.Fields.SentOn")]
        public DateTime? SentOn { get; set; }



        public IList<SelectListItem> AvailableCountries { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }
        public IList<SelectListItem> AvailableShippingMethods { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
        public IList<SelectListItem> AvailableWarehouses { get; set; }
    }
}