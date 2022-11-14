namespace customerSdmodule.Model1
{
    public partial class CustomerDetail
    {
        public string? PassportNo { get; set; }
        public string? Pan { get; set; }
        public byte? OccupationId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte? CustType { get; set; }
        public DateTime? RegDate { get; set; }
        public string? PhotoPath { get; set; }
        public string? EmailId { get; set; }
        public string CustId { get; set; } = null!;
        public byte? Age { get; set; }
        public byte? MaritalStatus { get; set; }
        public byte? NumMchild { get; set; }
        public byte? NumFchild { get; set; }
        public byte? Gender { get; set; }
        public byte? Citizen { get; set; }
        public int? EmpCode { get; set; }
        public byte? CountryId { get; set; }
        public bool? XplusStatus { get; set; }
        public byte? GlEnhance { get; set; }
        public byte? LandDtls { get; set; }
        public byte? LandCertId { get; set; }
        public DateTime? CerExpDate { get; set; }
        public string? LandCerNo { get; set; }
        public byte? Religion { get; set; }
        public byte? Caste { get; set; }
        public byte? Purposeofloan { get; set; }
        public bool? GstType { get; set; }
        public string? Gstin { get; set; }
        public string? OfficialEmailId { get; set; }
        public string? GuardianName { get; set; }
    }
}
