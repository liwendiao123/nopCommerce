using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.SMS.Aliyun.Domain;
using Nop.Plugin.SMS.Aliyun.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.SMS.Aliyun.Extensions
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        #region SmsTemplate

        public static SmsTemplateModel ToModel(this SmsTemplate entity)
        {
            return entity.MapTo<SmsTemplate, SmsTemplateModel>();
        }

        public static SmsTemplate ToEntity(this SmsTemplateModel model)
        {
            return model.MapTo<SmsTemplateModel, SmsTemplate>();
        }

        public static SmsTemplate ToEntity(this SmsTemplateModel model, SmsTemplate destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region SmsTemplate

        public static QueuedSmsModel ToModel(this QueuedSms entity)
        {
            return entity.MapTo<QueuedSms, QueuedSmsModel>();
        }

        public static QueuedSms ToEntity(this QueuedSmsModel model)
        {
            return model.MapTo<QueuedSmsModel, QueuedSms>();
        }

        public static QueuedSms ToEntity(this QueuedSmsModel model, QueuedSms destination)
        {
            return model.MapTo(destination);
        }

        #endregion
    }
}
