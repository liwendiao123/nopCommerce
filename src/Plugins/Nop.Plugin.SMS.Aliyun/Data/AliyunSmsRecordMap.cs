using Nop.Data.Mapping;
using Nop.Plugin.SMS.Aliyun.Domain;

namespace Nop.Plugin.SMS.Aliyun.Data
{
    public partial class AliyunSmsRecordMap : NopEntityTypeConfiguration<QueuedSms>
    {
        public AliyunSmsRecordMap()
        {
            this.ToTable("QueuedSms");
            this.HasKey(x => x.Id);

            this.Property(x => x.Body).HasMaxLength(4000);
        }
    }
}