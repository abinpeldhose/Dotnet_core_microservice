using System.ComponentModel.DataAnnotations;

namespace customerSdmodule.Model1
{
    public partial class BlockedDevice
    {
        [Key]
        public string? DeviceId { get; set; }
        public DateTime? LastAttemptDate { get; set; }
        public int? ActiveStatus { get; set; }
        public byte? Attempt { get; set; }
    }
}
