using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Nop.Core.Domain.AIBookModel;

namespace Nop.Data.Mapping.AIBookModel
{
  public  class BookNodeBookNodeTagMap : NopEntityTypeConfiguration<BookNodeBookNodeTagMapping>
    {
        public override void Configure(EntityTypeBuilder<BookNodeBookNodeTagMapping> builder)
        {

            builder.ToTable(NopMappingDefaults.BookNodeBookNodeTagTable);
            builder.HasKey(mapping => new { mapping.BookNodeId, mapping.BookNodeTagId });

            builder.Property(mapping => mapping.BookNodeId).HasColumnName("BookNodeId");
            builder.Property(mapping => mapping.BookNodeTagId).HasColumnName("BookNodeTagId");
            builder.HasOne(mapping => mapping.BookNode)
                    .WithMany(product => product.BookNodeBookNodeTagMappings)
                    .HasForeignKey(mapping => mapping.BookNodeId)
                    .IsRequired();
            builder.HasOne(mapping => mapping.BookNodeTag)
                .WithMany(productTag => productTag.BookNodeBookNodeTagMappings)
                .HasForeignKey(mapping => mapping.BookNodeTagId)
                .IsRequired();

            builder.Ignore(mapping => mapping.Id);

            base.Configure(builder);
        }
    }
}
