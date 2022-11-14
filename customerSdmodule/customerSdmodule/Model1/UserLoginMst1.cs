namespace customerSdmodule.Model1
{
    public partial class UserLoginMst1
    {
        public byte FirmId { get; set; }
        public int BranchId { get; set; }
        public string Custid { get; set; } = null!;
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public int? Status { get; set; }
        public string? Password { get; set; }
        public string Phone { get; set; } = null!;
        public string? SmsRefId { get; set; }
        public DateTime RegistartionDate { get; set; }
        public DateTime? PasswordUpdateDate { get; set; }
        public byte? MaxDay { get; set; }
        public byte? PasswordRules { get; set; }
        public string? Sharedby { get; set; }
        public string? Mpin { get; set; }
        public DateTime MpinDt { get; set; }
        public string? Imeinumber { get; set; }
        public string? Devicetoken { get; set; }
        public string? Appwebstatus { get; set; }
    }

}
