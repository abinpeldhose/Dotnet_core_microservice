namespace customerSdmodule.Model1
{
    public partial class SdAgentMaster
    {
        public int AgentId { get; set; }
        public string AgentName { get; set; } = null!;
        public string HouseName { get; set; } = null!;
        public byte StateId { get; set; }
        public int Pinserial { get; set; }
        public string? Pan { get; set; }
        public DateTime Dob { get; set; }
        public string? PhoneNo { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public byte BranchId { get; set; }
        public string StatusId { get; set; } = null!;
        public string? StreetName { get; set; }
        public string? LandMark { get; set; }
        public byte? ProfessionId { get; set; }
        public byte? NomineeId { get; set; }
        public string? UserId { get; set; }
        public byte? Language { get; set; }
        public DateTime? TraDt { get; set; }
        public byte? FirmId { get; set; }
        public int? OldId { get; set; }
        public int? CsaId { get; set; }
        public string? PanStatus { get; set; }
        public int? RefAgentId { get; set; }
        public bool? GstType { get; set; }
        public string? Gstin { get; set; }
    }
}
