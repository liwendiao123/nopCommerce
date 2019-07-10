using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Core.Domain.Messages.SMS
{
   public class SmsMsgRecord:BaseEntity
    {
        public string AppId { get; set; }
        public string SysName { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public string TemplateCode { get; set; }   
        public string Content { get; set; }
        public int Type { get; set; }
        public int IsRead { get; set; }
        public DateTime CreateTime { get; set; }
       
    }
}
