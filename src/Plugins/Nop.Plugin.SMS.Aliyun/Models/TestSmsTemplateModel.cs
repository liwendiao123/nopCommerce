using FluentValidation.Attributes;
using Microsoft.AspNetCore.Http;
using Nop.Plugin.SMS.Aliyun.Validators;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.Models;
using System.Collections.Generic;

namespace Nop.Plugin.SMS.Aliyun.Models
{
    [Validator(typeof(TestSmsTemplateValidator))]
    public partial class TestSmsTemplateModel : BaseNopEntityModel
    {
        //MVC is suppressing further validation if the IFormCollection is passed to a controller method. That's why we add to the model
        public IFormCollection Form { get; set; }

        public TestSmsTemplateModel()
        {
            Tokens = new List<string>();
        }

        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Test.Tokens")]
        public List<string> Tokens { get; set; }

        [NopResourceDisplayName("Admin.ContentManagement.MessageTemplates.Test.SendTo")]
        public string SendTo { get; set; }
    }
}
