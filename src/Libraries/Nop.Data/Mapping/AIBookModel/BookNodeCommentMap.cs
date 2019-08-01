using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.AIBookModel;
using Microsoft.EntityFrameworkCore;

namespace Nop.Data.Mapping.AIBookModel
{
   public class BookNodeCommentMap : NopEntityTypeConfiguration<BookNodeComment>
    {
        public override void Configure(EntityTypeBuilder<BookNodeComment> builder)
        {

            builder.ToTable(nameof(BookNodeComment));

            builder.HasKey(comment => comment.Id);
            builder.HasOne(comment => comment.BookNodeItem)
                .WithMany(news => news.BookNodesComments)
                .HasForeignKey(comment => comment.BookNodeId)
                .IsRequired();

            builder.HasOne(comment => comment.Customer)
                .WithMany()
                .HasForeignKey(comment => comment.CustomerId)
                .IsRequired();

            builder.HasOne(comment => comment.Store)
                .WithMany()
                .HasForeignKey(comment => comment.StoreId)
                .IsRequired();
            base.Configure(builder);
        }

    }
}
