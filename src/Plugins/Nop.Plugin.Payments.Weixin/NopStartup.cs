using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Senparc.Weixin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.WeixinPay
{
   public class NopStartup:INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)

        {

            //添加Senparc.Weixin配置文件（内容可以根据需要对应修改）
            services.Configure<SenparcWeixinSetting>(configuration.GetSection("SenparcWeixinSetting"));


        }


        public void Configure(IApplicationBuilder application)
        {
            
        }

        public int Order
        {
            get { return 1001; }
        }

    }
}
