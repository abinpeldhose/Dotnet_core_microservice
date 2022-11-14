using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class SdStatusMaster
    {
        public byte StatusId { get; set; }
        public string Status { get; set; } = null!;
        public string TableName { get; set; } = null!;
        public string ColumnName { get; set; } = null!;
        public string? Debit { get; set; }
        public string? Credit { get; set; }
    }
}
