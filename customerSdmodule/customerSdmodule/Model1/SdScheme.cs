using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class SdScheme
    {
        public byte FirmId { get; set; }
        public short BranchId { get; set; }
        public byte ModuleId { get; set; }
        public int? SchemeStatus { get; set; }
        public string? Scheme { get; set; }
        public short SchemeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public byte? IntProc { get; set; }
        public string? IntPay { get; set; }
        public byte? MinPeriod { get; set; }
        public long? MinAmount { get; set; }
        public long? MaxAmount { get; set; }

        public virtual BranchMaster Branch { get; set; } = null!;
        public virtual FirmMaster Firm { get; set; } = null!;
        public virtual StatusMaster? SchemeStatusNavigation { get; set; }
    }
}
