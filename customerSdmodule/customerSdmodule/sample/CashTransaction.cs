using System;
using System.Collections.Generic;

namespace customerSdmodule.sample
{
    public partial class CashTransaction
    {
        public byte? FirmId { get; set; }
        public byte? BranchId { get; set; }
        public string? CustName { get; set; }
        public string? Descr { get; set; }
        public int? TransNo { get; set; }
        public decimal? Amount { get; set; }
        public string? Type { get; set; }
        public DateTime? TraDt { get; set; }
        public int? AccountNo { get; set; }
        public int? CashId { get; set; }
        public int? TransId { get; set; }
        public bool? Counter { get; set; }
        public string? CustId { get; set; }
        public string? RefId { get; set; }
    }
}
