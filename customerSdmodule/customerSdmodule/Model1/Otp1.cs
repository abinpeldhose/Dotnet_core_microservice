using System.ComponentModel.DataAnnotations;

namespace customerSdmodule.Model1
{
    public partial class Otp1
    {
        public int? TransactionId { get; set; }
        public string? UserId { get; set; }
        public string Mobilenumber { get; set; } = null!;
        public int? Otp { get; set; }
        [Key]
        public DateTime TimeStamp { get; set; }
        public byte? Status { get; set; }
        public byte? MaxTime { get; set; }
    }
}
