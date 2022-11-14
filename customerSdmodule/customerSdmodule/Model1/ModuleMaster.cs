using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class ModuleMaster
    {
        public byte FirmId { get; set; }
        public short BranchId { get; set; }
        public short ModuleId { get; set; }
        public string ModuleDescr { get; set; } = null!;
        public string ModuleAbbr { get; set; } = null!;
        public string? AddedBy { get; set; }

        public virtual BranchMaster Branch { get; set; } = null!;
        public virtual FirmMaster Firm { get; set; } = null!;
    }
}
