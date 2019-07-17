using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Customers;

namespace Nop.Data.Mapping.Customers
{
    public class CustomerBookMap : NopEntityTypeConfiguration<CustomerBook>
    {

        public override void Configure(EntityTypeBuilder<CustomerBook> builder)
        {
            base.Configure(builder);
        }
    }
}
