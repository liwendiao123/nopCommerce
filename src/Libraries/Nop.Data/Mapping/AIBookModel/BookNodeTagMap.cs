using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.AIBookModel;

namespace Nop.Data.Mapping.AIBookModel
{
  public  class BookNodeTagMap : NopEntityTypeConfiguration<BookNodeTag>
    {
        public override void Configure(EntityTypeBuilder<BookNodeTag> builder)
        {
            builder.ToTable(nameof(BookNodeTag));
            builder.HasKey(productTag => productTag.Id);

            builder.Property(productTag => productTag.Name).HasMaxLength(400).IsRequired();

            base.Configure(builder);
        }
    }
}
