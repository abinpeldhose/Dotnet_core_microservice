using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class TdsMaster
    {
        public byte StatusId { get; set; }
        public string Status { get; set; } = null!;
        public byte TdsCode { get; set; }
        public decimal TdsRate { get; set; }
    }
}
