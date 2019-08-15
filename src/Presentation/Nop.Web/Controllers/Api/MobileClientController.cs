using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Configuration;
using Nop.Web.Models.Api;
namespace Nop.Web.Controllers.Api
{
    public class MobileClientController : BasePublicController
    {

        private readonly NopConfig _config;
        public MobileClientController(NopConfig config)
        {
            _config = config;
        }
        public ActionResult GetVersion()
        {
            return Json(new MobileClientModel
            {
                AndroidApkUrl = _config.AndroidApkUrl,
                AndroidClientVersion = _config.AndroidClientVersion,
                IosClientVersion = _config.IosClientVersion,
                IosPackageUrl = _config.IosPackageUrl,
                PcClientPackageUrl = _config.PcClientPackageUrl,
                PcClientVersion = _config.PcClientVersion,
                CreateTime = DateTime.Now,
                LastUpdateTime = DateTime.Now
            });
        }





    }
}
