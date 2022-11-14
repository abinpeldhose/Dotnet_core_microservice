using System;
using System.Collections.Generic;

namespace customerSdmodule.Sample1
{
    public partial class KeyMaster
    {
        public byte FirmId { get; set; }
        public byte BranchId { get; set; }
        public byte ModuleId { get; set; }
        public int KeyId { get; set; }
        public string? Value { get; set; }
        public string? Description { get; set; }
    }
}
