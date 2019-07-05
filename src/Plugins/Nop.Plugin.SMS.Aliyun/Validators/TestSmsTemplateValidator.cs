using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Nop.Plugin.SMS.Aliyun.Models;

namespace Nop.Plugin.SMS.Aliyun.Validators
{

    public partial class TestSmsTemplateValidator : BaseNopValidator<TestSmsTemplateModel>
    {
        public TestSmsTemplateValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.SendTo).NotEmpty();
            RuleFor(x => x.SendTo).EmailAddress().WithMessage(localizationService.GetResource("Admin.Common.WrongEmail"));
        }
    }
}
