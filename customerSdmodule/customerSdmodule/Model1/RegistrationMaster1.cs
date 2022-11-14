namespace customerSdmodule.Model1
{
    public partial class RegistrationMaster1
    {
        public byte FirmId { get; set; }
        public int BranchId { get; set; }
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string? Password { get; set; }
        public string Phone { get; set; } = null!;
        public DateTime RegistartionDate { get; set; }
        public DateTime? PasswordUpdateDate { get; set; }
        public byte? MaxDay { get; set; }
        public byte? PasswordRules { get; set; }
    }
}
