using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace customerSdmodule.Model1
{
    public partial class Otp
    {
        public int? TransactionId { get; set; }
        public string UserId { get; set; } = null!;
        public string Mobilenumber { get; set; } = null!;
        public string? Otp1 { get; set; }
        [Key]
        public DateTime TimeStamp { get; set; }
        public byte? Status { get; set; }
        public byte? MaxTime { get; set; }
    }
}
