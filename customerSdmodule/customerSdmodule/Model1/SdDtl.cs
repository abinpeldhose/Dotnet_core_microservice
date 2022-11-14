using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class SdDtl
    {
        public string DepositId { get; set; } = null!;
        public bool? Repayable { get; set; }
        public DateTime? NopDate { get; set; }
        public bool? ResiStat { get; set; }
        public string? SecAppl { get; set; }
        public string? ThrdAppl { get; set; }
        public DateTime? MinorDob { get; set; }
        public string? SecApplAddress { get; set; }
        public string? ThirdApplAddress { get; set; }
        public string? OtherAppl { get; set; }
    }
}
