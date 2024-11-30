using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSAPIBULKSMSBD.Models
{
    public class Outbox_API_ADN_TEST
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [StringLength(50)]
        public string? Sender { get; set; }

        [StringLength(50)]
        public string? Receiver { get; set; }

        [StringLength(2000)]
        public string? Message { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DatabaseSubmitTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ChargingTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DeliveryTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? SentTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ActualSentTime { get; set; }

        [StringLength(50)]
        public string? RequestId { get; set; }

        [StringLength(500)]
        public string? SmScMsgId { get; set; }

        public short? Status { get; set; }

        public short? IsPollProcessed { get; set; }

        public short? SmsCount { get; set; }

        public int? ErrorCode { get; set; }

        public short? SubmitResponseCount { get; set; }

        [StringLength(6)]
        public string? Charge { get; set; }

        [StringLength(50)]
        public string? ServiceId { get; set; }

        [StringLength(50)]
        public string? ServiceType { get; set; }

        public short? IsCharged { get; set; }

        [StringLength(1500)]
        public string? ApiResponse { get; set; }

        public decimal? InboxId { get; set; }

        public short? RetryCount { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? RetryTime { get; set; }

        public int? Priority { get; set; }
    }
}
