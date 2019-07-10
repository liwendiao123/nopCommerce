using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Messages.SMS;
using Microsoft.EntityFrameworkCore;

using Nop.Core.Domain.Messages;
namespace Nop.Data.Mapping.Messages
{
  public  class SmsMsgRecordMap : NopEntityTypeConfiguration<SmsMsgRecord>
    {

        public override void Configure(EntityTypeBuilder<SmsMsgRecord> builder)
        {
            builder.ToTable(nameof(SmsMsgRecord));
            builder.HasKey(template => template.Id);
            base.Configure(builder);
        }
    }
}
