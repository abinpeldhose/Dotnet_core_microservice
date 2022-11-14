using System.ComponentModel.DataAnnotations;

namespace customerSdmodule.Model1
{
    public partial class SdScheduleMaster
    {
        [Key]
        public decimal? RtId { get; set; }
        public byte? FirmId { get; set; }
        public short? BranchId { get; set; }
        public byte? ModuleId { get; set; }
        public string? DepositId { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public byte? NoOccurance { get; set; }
        public decimal? Amount { get; set; }
        public string? Type { get; set; }
        public string? Frequency { get; set; }
        public int? StatusId { get; set; }
        public string? Ifsc { get; set; }
        public string? AccountNumber { get; set; }
        public int? UserType { get; set; }
        public string? UserId { get; set; }
    }
}
