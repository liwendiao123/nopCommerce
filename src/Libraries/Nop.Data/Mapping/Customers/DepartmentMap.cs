using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Nop.Data.Mapping.Customers
{
   public  class DepartmentMap : NopEntityTypeConfiguration<Department>
    {

        public override void Configure(EntityTypeBuilder<Department> builder)
        {

            builder.ToTable(nameof(Department));
            builder.HasKey(dep => dep.Id);
            base.Configure(builder);
        }

    }
}
