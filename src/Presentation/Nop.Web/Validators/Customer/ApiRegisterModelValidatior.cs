using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Web.Models.Customer;
using FluentValidation;
namespace Nop.Web.Validators.Customer
{
    public class ApiRegisterModelValidatior : BaseNopValidator<ApiRegisterModel>
    {
        public ApiRegisterModelValidatior(ILocalizationService localizationService,
            IStateProvinceService stateProvinceService,
            CustomerSettings customerSettings)
        {

            //  RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.Email.Required"));
            //  RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
           
                RuleFor(x => x.UserName).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.Username.Required"));
                RuleFor(x => x.UserName).IsUsername(customerSettings).WithMessage(localizationService.GetResource("Account.Fields.Username.NotValid"));
                RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.LastName.Required"));
            RuleFor(x => x.Password).IsPassword(localizationService, customerSettings);

            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.ConfirmPassword.Required"));
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage(localizationService.GetResource("Account.Fields.Password.EnteredPasswordsDoNotMatch"));
            RuleFor(x => x.Phone).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.Phone.Required"));
            RuleFor(x => x.SmsCode).NotEmpty().WithMessage("手机验证码不能为空");

        }

    }
}
