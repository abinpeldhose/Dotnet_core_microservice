using System;
using System.Collections.Generic;

namespace customerSdmodule.Sample1
{
    public partial class TransactionDetail
    {
        public byte FirmId { get; set; }
        public byte BranchId { get; set; }
        public byte ModuleId { get; set; }
        public int Transno { get; set; }
        public string RefId { get; set; } = null!;
        public int AccountNo { get; set; }
        public string Type { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime TraDt { get; set; }
        public string? PayMode { get; set; }
        public DateTime ValueDt { get; set; }
        public string? Descr { get; set; }
        public string? UserId { get; set; }
        public int TransId { get; set; }
        public int? ContraNo { get; set; }
        public string? Narration { get; set; }
        public byte? SegmentId { get; set; }
    }
}
