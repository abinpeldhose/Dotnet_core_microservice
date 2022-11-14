using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class RegistrationMaster
    {
        public byte FirmId { get; set; }
        public short BranchId { get; set; }
        public int UserId { get; set; }
        public string? Password { get; set; }
        public string Phone { get; set; } = null!;
        public DateTime RegistartionDate { get; set; }
        public DateTime? PasswordUpdateDate { get; set; }
    }
}
