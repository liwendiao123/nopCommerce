using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Nop.Core.Domain.AIBookModel;



namespace Nop.Data.Mapping.AIBookModel
{
   public partial class AiBookModelMap : NopEntityTypeConfiguration<AiBookModel>
    {
        public override void Configure(EntityTypeBuilder<AiBookModel> builder)
        {

            builder.ToTable(nameof(AiBookModel));
            builder.HasKey(aibookmodel => aibookmodel.Id);


           // builder.HasKey(comment => comment.Id);

            builder.HasOne(aibookmodel => aibookmodel.BookDir)
                .WithMany(aibookmodel => aibookmodel.AiBookModels)
                .HasForeignKey(comment => comment.BookDirID)
                .IsRequired();

            //builder.HasOne(aibook => aibook.BlogPost)
            //    .WithMany(blog => blog.BlogComments)
            //    .HasForeignKey(comment => comment.BlogPostId)
            //    .IsRequired();

            base.Configure(builder);
        }
    }
}
