using Nop.Data.Mapping;
using Nop.Plugin.SMS.Aliyun.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.SMS.Aliyun.Data
{

    public partial class SmsTemplateMap : NopEntityTypeConfiguration<SmsTemplate>
    {
        public SmsTemplateMap()
        {
            this.ToTable("SmsTemplate");
            this.HasKey(x => x.Id);

            this.Property(x => x.Body).HasMaxLength(4000);
        }
    }
}
