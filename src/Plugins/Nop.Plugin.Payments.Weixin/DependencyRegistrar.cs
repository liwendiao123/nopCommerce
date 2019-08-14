using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;


namespace Nop.Plugin.Payments.WeixinPay
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            try
            {

                /*
                var tenPayV3_MchId =  Senparc.Weixin.Config.DefaultSenparcWeixinSetting.TenPayV3_MchId;
                var tenPayV3_Key = Senparc.Weixin.Config.DefaultSenparcWeixinSetting.TenPayV3_Key;
                var tenPayV3_AppId = Senparc.Weixin.Config.DefaultSenparcWeixinSetting.TenPayV3_AppId;
                var tenPayV3_AppSecret = Senparc.Weixin.Config.DefaultSenparcWeixinSetting.TenPayV3_AppSecret;
                var tenPayV3_TenpayNotify = Senparc.Weixin.Config.DefaultSenparcWeixinSetting.TenPayV3_TenpayNotify;
                
                var tenPayV3Info = new TenPayV3Info(tenPayV3_AppId, tenPayV3_AppSecret, tenPayV3_MchId, tenPayV3_Key,
                                                    tenPayV3_TenpayNotify);
                TenPayV3InfoCollection.Register(tenPayV3Info);

                */
            }
            catch {
            }

        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order
        {
            get { return 1; }
        }
    }
}
