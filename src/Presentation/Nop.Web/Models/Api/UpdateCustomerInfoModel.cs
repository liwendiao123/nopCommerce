using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Models.Api
{
    public class UpdateCustomerInfoModel
    {

        public int Id { get; set; }

        [NopResourceDisplayName("Account.Fields.Phone")]
        public string Phone { get; set; }

        [NopResourceDisplayName("Account.Fields.Username")]
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        [NopResourceDisplayName("Account.Fields.Email")]
        public string Email { get; set; }

        [NopResourceDisplayName("Account.Fields.Gender")]
         public string Gender { get; set; }
        public int DepId { get; set; }
        public int RoleNameId { get; set; }
        public string Imgurl { get; set; }
        public string InviteCode{get;set;}
        public int PictureId { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
    }
}
