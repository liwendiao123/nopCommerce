using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Customers;

namespace Nop.Data.Mapping.Customers
{
   public class CustomerBookNodeMap : NopEntityTypeConfiguration<CustomerBookNode>
    {
        public override void Configure(EntityTypeBuilder<CustomerBookNode> builder)
        {
            builder.ToTable(nameof(CustomerBookNode));
            builder.HasKey(mapping => new { mapping.BookNodeId, mapping.CustomerId });

            //builder.Property(mapping => mapping.CustomerId).HasColumnName("CustomerId");
            //builder.Property(mapping => mapping.ProductId).HasColumnName("ProductId");


            builder.HasOne(mapping => mapping.Customer)
               .WithMany(customer => customer.CustomerBookNodes)
               .HasForeignKey(mapping => mapping.CustomerId)
               .IsRequired();
            builder.HasOne(mapping => mapping.Product)
                .WithMany()
                .HasForeignKey(mapping => mapping.BookNodeId)
                .IsRequired();
            builder.Ignore(mapping => mapping.Id);
            base.Configure(builder);
        }
    }
}
