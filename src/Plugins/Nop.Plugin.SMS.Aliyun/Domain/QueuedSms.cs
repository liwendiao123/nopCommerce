using Nop.Core;
using System;

namespace Nop.Plugin.SMS.Aliyun.Domain
{
    /// <summary>
    /// 表示已发送的某条短信
    /// </summary>
    public partial class QueuedSms : BaseEntity
    {
        /// <summary>
        /// Gets or sets the priority
        /// </summary>
        public int PriorityId { get; set; }

        /// <summary>
        /// Gets or sets the To property (email address)
        /// </summary>
        public string ToMobileNumber { get; set; }

        /// <summary>
        /// Gets or sets the ToName property
        /// </summary>
        public string ToName { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        public string Subject { get; set; }

        public string TemplateCode { get; set; }
        public string TemplateParamJson { get; set; }

        public Guid OutGuid { get; set; }

        /// <summary>
        /// Gets or sets the body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the date and time of item creation in UTC
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time in UTC before which this email should not be sent
        /// </summary>
        public DateTime? DontSendBeforeDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the send tries
        /// </summary>
        public int SentTries { get; set; }

        /// <summary>
        /// Gets or sets the sent date and time
        /// </summary>
        public DateTime? SentOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the priority
        /// </summary>
        public QueuedSmsPriority Priority
        {
            get
            {
                return (QueuedSmsPriority)this.PriorityId;
            }
            set
            {
                this.PriorityId = (int)value;
            }
        }
    }

    public enum QueuedSmsPriority
    {
        /// <summary>
        /// Low
        /// </summary>
        Low = 0,
        /// <summary>
        /// High
        /// </summary>
        High = 5
    }
}