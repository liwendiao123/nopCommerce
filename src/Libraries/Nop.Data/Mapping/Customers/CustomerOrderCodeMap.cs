using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Customers;

namespace Nop.Data.Mapping.Customers
{
   public class CustomerOrderCodeMap : NopEntityTypeConfiguration<CustomerOrderCode>
    {
        public override void Configure(EntityTypeBuilder<CustomerOrderCode> builder)
        {
            builder.ToTable(nameof(CustomerOrderCode));
            builder.HasOne(mapping => mapping.Customer)
               .WithMany(customer => customer.CustomerOrderCodes)
               .HasForeignKey(mapping => mapping.CustomerId)
               .IsRequired();
            base.Configure(builder);
        }

    }
}
