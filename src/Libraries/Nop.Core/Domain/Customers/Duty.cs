using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Core.Domain.Customers
{
  public  class Duty:BaseEntity
    {
        public string Name { get; set; }


       public int DisplayOrder { get; set; }
    }
}
