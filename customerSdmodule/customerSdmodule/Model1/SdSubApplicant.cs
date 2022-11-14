using System.ComponentModel.DataAnnotations;

namespace customerSdmodule.Model1
{
    public partial class SdSubApplicant
    {
        public string? Name { get; set; }
        public string? FatHus { get; set; }
        public string? House { get; set; }
        public string? Location { get; set; }
        public DateTime? Dob { get; set; }
        public string? Relation { get; set; }
        public DateTime? CancelDt { get; set; }
        public string NomineeId { get; set; } = null!;
        public string DocumentId { get; set; } = null!;
        public int Category { get; set; }
        public int? FirmId { get; set; }
        public short BranchId { get; set; }
        public byte? ModuleId { get; set; }
        public string? Adduser { get; set; }
        public string? Deluser { get; set; }
        public string? CustId { get; set; }
        public string? Phone { get; set; }
        public byte? SubType { get; set; }
        public string? MinorGuardian { get; set; }
        public byte? MinorStatus { get; set; }
    }
}
