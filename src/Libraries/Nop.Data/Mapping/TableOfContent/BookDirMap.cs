using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.TableOfContent;
using Microsoft.EntityFrameworkCore;

namespace Nop.Data.Mapping.TableOfContent
{
    public class BookDirMap : NopEntityTypeConfiguration<BookDir>
    {
        public override void Configure(EntityTypeBuilder<BookDir> builder)
        {
            builder.ToTable(nameof(BookDir));
            builder.HasKey(dir => dir.Id);

            builder.Property(dir => dir.Name).HasMaxLength(400).IsRequired();
            builder.Property(dir => dir.Description);
            
            builder.Property(dir => dir.MetaKeywords).HasMaxLength(1024);
            builder.Property(dir => dir.MetaKeywords).HasMaxLength(1024);
            builder.Property(dir => dir.MetaTitle).HasMaxLength(1024);
           

     
            base.Configure(builder);
        }
    }
}
