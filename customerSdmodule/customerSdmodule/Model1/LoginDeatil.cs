using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class LoginDeatil
    {
        public string SessionId { get; set; } = null!;
        public byte FirmId { get; set; }
        public short BranchId { get; set; }
        public int UserId { get; set; }
        public DateTime LoginDate { get; set; }
    }
}
