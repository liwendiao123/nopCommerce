using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Customers;
using Microsoft.EntityFrameworkCore;
namespace Nop.Data.Mapping.Customers
{
    public class CustomerBookMap : NopEntityTypeConfiguration<CustomerBook>
    {

        public override void Configure(EntityTypeBuilder<CustomerBook> builder)
        {
            builder.ToTable(nameof(CustomerBook));
            builder.HasKey(mapping => new { mapping.ProductId, mapping.CustomerId });
            //builder.Property(mapping => mapping.CustomerId).HasColumnName("CustomerId");
            //builder.Property(mapping => mapping.ProductId).HasColumnName("ProductId");
            builder.HasOne(mapping => mapping.Customer)
               .WithMany(customer => customer.CustomerBooks)
               .HasForeignKey(mapping => mapping.CustomerId)
               .IsRequired();
            builder.HasOne(mapping => mapping.Product)
                .WithMany()
                .HasForeignKey(mapping => mapping.ProductId)
                .IsRequired();
            builder.Ignore(mapping => mapping.Id);
            base.Configure(builder);
        }
    }
}
