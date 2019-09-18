using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api.WebApiModel.ApiBuyBook
{
    public class GetBarCodeJsonRequest:ApiBaseRequest
    {
            public int orderId { get; set; }
            
            public string inviteCode { get; set; }
    }
}
