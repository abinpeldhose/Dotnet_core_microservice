using System;
using System.Collections.Generic;

namespace customerSdmodule.sample
{
    public partial class AccountStatus
    {
        public byte FirmId { get; set; }
        public byte BranchId { get; set; }
        public int AccountNo { get; set; }
        public bool? StatusId { get; set; }
    }
}
