using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.SMS.Aliyun.Data;
using Nop.Plugin.SMS.Aliyun.Domain;
using Nop.Plugin.SMS.Aliyun.Services;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.SMS.Aliyun.Infrastructure
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
            builder.RegisterType<JsonTokenizer>().As<IJsonTokenizer>().InstancePerLifetimeScope();

            builder.RegisterType<SmsTemplateService>().As<ISmsTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<QueuedSmsService>().As<IQueuedSmsService>().InstancePerLifetimeScope();
            
            builder.RegisterType<ShortMessageTokenProvider>().As<IShortMessageTokenProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ShortMessageService>().As<IShortMessageService>().InstancePerLifetimeScope();
            
            //data context
            this.RegisterPluginDataContext<AliyunSmsObjectContext>(builder, "nop_object_context_sms_aliyun");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<SmsTemplate>>()
                .As<IRepository<SmsTemplate>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_sms_aliyun"))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<QueuedSms>>()
                .As<IRepository<QueuedSms>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_sms_aliyun"))
                .InstancePerLifetimeScope();
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
