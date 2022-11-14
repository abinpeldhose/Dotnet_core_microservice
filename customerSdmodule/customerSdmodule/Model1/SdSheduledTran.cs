using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace customerSdmodule.Model1
{
    public partial class SdSheduledTran
    {
        public byte FirmId { get; set; }
        public short BranchId { get; set; }
        public string DepositId { get; set; } = null!;
        public DateTime? TraDt { get; set; }
        public string? Descr { get; set; }
        public decimal Amount { get; set; }
        public string? Type { get; set; }
        public byte? Status { get; set; }
        public decimal? RtId { get; set; }
    }
}
