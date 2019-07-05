using Nop.Data;
using Nop.Plugin.SMS.Aliyun.Domain;
using Nop.Plugin.SMS.Aliyun.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Nop.Plugin.SMS.Aliyun.Validators
{
    public partial class SmsTemplateValidator : BaseNopValidator<SmsTemplateModel>
    {
        public SmsTemplateValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Subject).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Fields.Subject.Required"));
            RuleFor(x => x.Body).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Fields.Body.Required"));

            SetDatabaseValidationRules<SmsTemplate>(dbContext);
        }
    }
}
