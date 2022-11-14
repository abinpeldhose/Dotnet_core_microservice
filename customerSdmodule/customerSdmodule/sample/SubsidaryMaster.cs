using System;
using System.Collections.Generic;

namespace customerSdmodule.sample
{
    public partial class SubsidaryMaster
    {
        public byte FirmId { get; set; }
        public byte BranchId { get; set; }
        public int ParentAcc { get; set; }
        public int AccountNo { get; set; }
        public string? AccountName { get; set; }
        public decimal? Balance { get; set; }
        public string? Type { get; set; }
        public byte? StatusId { get; set; }
        public byte? SubId { get; set; }
    }
}
