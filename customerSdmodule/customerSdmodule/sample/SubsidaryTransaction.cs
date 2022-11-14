using System;
using System.Collections.Generic;

namespace customerSdmodule.sample
{
    public partial class SubsidaryTransaction
    {
        public byte FirmId { get; set; }
        public byte BranchId { get; set; }
        public int ParentAcc { get; set; }
        public int AccountNo { get; set; }
        public int? TransNo { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = null!;
        public DateTime? TraDt { get; set; }
        public string? PayMode { get; set; }
        public string? Descr { get; set; }
        public int? TransId { get; set; }
        public int? SubId { get; set; }
    }
}
