using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Models.Api.WebApiModel
{
    public class ChangePasswordJsonRequest:ApiBaseRequest
    {
        public string userName { get;set;
        }

        [NoTrim]
        [DataType(DataType.Password)]
        [NopResourceDisplayName("Account.ChangePassword.Fields.OldPassword")]
        public string OldPassword { get; set; }

        [NoTrim]
        [DataType(DataType.Password)]
        [NopResourceDisplayName("Account.ChangePassword.Fields.NewPassword")]
        public string NewPassword { get; set; }

        [NoTrim]
        [DataType(DataType.Password)]
        [NopResourceDisplayName("Account.ChangePassword.Fields.ConfirmNewPassword")]
        public string ConfirmNewPassword { get; set; }

        public string Result { get; set; }
    }
}
