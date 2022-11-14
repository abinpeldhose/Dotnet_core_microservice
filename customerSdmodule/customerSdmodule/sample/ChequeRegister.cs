using System;
using System.Collections.Generic;

namespace customerSdmodule.sample
{
    public partial class ChequeRegister
    {
        public byte? FirmId { get; set; }
        public byte? BranchId { get; set; }
        public string? ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public byte? StatusId { get; set; }
        public string? Type { get; set; }
        public string? BankName { get; set; }
        public int? BankCode { get; set; }
        public DateTime? TraDt { get; set; }
        public int? TransId { get; set; }
        public decimal? ChequeAmount { get; set; }
        public string? Descr { get; set; }
        public int? TransNo { get; set; }
    }
}
