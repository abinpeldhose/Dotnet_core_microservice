using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class GeneralParameter
    {
        public byte FirmId { get; set; }
        public byte ParmtrId { get; set; }
        public string? ParmtrName { get; set; }
        public string? ParmtrValue { get; set; }
        public byte ModuleId { get; set; }
        public string? AccountType { get; set; }
        public string? SubLedger { get; set; }

        public virtual FirmMaster Firm { get; set; } = null!;
    }
}
