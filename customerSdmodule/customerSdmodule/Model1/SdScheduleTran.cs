using System.ComponentModel.DataAnnotations;

namespace customerSdmodule.Model1
{
    public partial class SdScheduleTran
    {
        public decimal RtId { get; set; }
        public byte FirmId { get; set; }
        public short BranchId { get; set; }
        public byte ModuleId { get; set; }
        public string DepositId { get; set; } = null!;
        public DateTime TraDt { get; set; }
        public decimal Amount { get; set; }
        public string? Type { get; set; }
        public byte? StatusId { get; set; }
        public string? Ifsc { get; set; }
        public string? AccountNumber { get; set; }
        public int? UserType { get; set; }
        public string? UserId { get; set; }
        public int? TransId { get; set; }
        public DateTime? CloseDate { get; set; }
        public string? BhId { get; set; }
    }
}
