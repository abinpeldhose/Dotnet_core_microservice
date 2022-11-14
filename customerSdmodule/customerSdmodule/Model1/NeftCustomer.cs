namespace customerSdmodule.Model1
{
    public partial class NeftCustomer
    {
        public byte? BranchId { get; set; }
        public byte? FirmId { get; set; }
        public string? CustId { get; set; }
        public string? CustName { get; set; }
        public string? IfscCode { get; set; }
        public string? BeneficiaryBranch { get; set; }
        public string BeneficiaryAccount { get; set; } = null!;
        public byte? AccType { get; set; }
        public string? VerifyStatus { get; set; }
        public string? UserId { get; set; }
        public string? RejectReason { get; set; }
        public long? MobileNumber { get; set; }
        public string? ReasonPhone { get; set; }
        public DateTime? TraDt { get; set; }
        public short? BankId { get; set; }
        public DateTime? ModifyDt { get; set; }
        public byte? Moduleid { get; set; }
        public bool? Status { get; set; }
        public string? CustRefId { get; set; }
        public string? VerifiedBy { get; set; }
        public DateTime? VerifiedDt { get; set; }
        public decimal? AttachmentType { get; set; }
    }
}
