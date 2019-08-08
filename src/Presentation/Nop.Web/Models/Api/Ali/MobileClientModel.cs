using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Api
{
    public class MobileClientModel
    {
        public string AndroidApkUrl { get; set; }
        public string AndroidClientVersion { get; set; }
        public string IosPackageUrl { get; set; }
        public string IosClientVersion { get; set; }

        public string PcClientVersion { get; set; }

        public string PcClientPackageUrl { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }

    }
}
