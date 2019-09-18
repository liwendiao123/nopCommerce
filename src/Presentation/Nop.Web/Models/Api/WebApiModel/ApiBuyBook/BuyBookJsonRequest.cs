using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api.WebApiModel.ApiBuyBook
{
    public class BuyBookJsonRequest:WebApiModel.ApiBaseRequest
    {
           public string token { get; set; }
           public string pid { get; set; }
        public string qs_clientid { get; set; }
        public int paymethod { get; set; }
        public string inviteCode { get; set; }
    }
}
