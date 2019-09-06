using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Customers;

namespace Nop.Data.Mapping.Customers
{
   public class CustomerBookDirMap : NopEntityTypeConfiguration<CustomerBookNodeLog>
    {
        public override void Configure(EntityTypeBuilder<CustomerBookNodeLog> builder)
        {
            builder.ToTable(nameof(CustomerBookNodeLog));
            builder.HasKey(mapping => new { mapping.BookNodeId, mapping.CustomerId });
            //builder.Property(mapping => mapping.CustomerId).HasColumnName("CustomerId");
            //builder.Property(mapping => mapping.ProductId).HasColumnName("ProductId");
            builder.HasOne(mapping => mapping.Customer)
               .WithMany(customer => customer.CustomerBookNodeLogs)
               .HasForeignKey(mapping => mapping.CustomerId)
               .IsRequired();

            builder.HasOne(mapping => mapping.BookNode)
                .WithMany()
                .HasForeignKey(mapping => mapping.BookNodeId)
                .IsRequired();
            builder.Ignore(mapping => mapping.Id);
            base.Configure(builder);
        }
    }
}
