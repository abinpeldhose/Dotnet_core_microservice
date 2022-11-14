using System;
using System.Collections.Generic;

namespace customerSdmodule.Sample1
{
    public partial class AccountMaster
    {
        public byte FirmId { get; set; }
        public byte BranchId { get; set; }
        public int AccountNo { get; set; }
        public decimal? Balance { get; set; }
        public string? Type { get; set; }
    }
}
