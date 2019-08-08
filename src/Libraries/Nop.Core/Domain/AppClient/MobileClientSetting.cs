using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Configuration;

namespace Nop.Core.Domain.AppClient
{
   public class MobileClientSetting : ISettings
    {
        public string AndroidApkUrl { get; set; }
        public string AndroidClientVersion { get; set; }
        public string IosPackageUrl { get; set; }
        public string IosClientVersion { get; set; }

        public DateTime CreateTime { get; set; }


        public DateTime LastUpdateTime { get; set; }

        public void SetUpdateTime()
        {
            this.LastUpdateTime = DateTime.Now;
        }
    }
}
