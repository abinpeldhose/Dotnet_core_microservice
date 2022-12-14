using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace customerSdmodule.Model1
{
    public partial class SdTran
    {
        public byte FirmId { get; set; }
        [Key]
        public short BranchId { get; set; }
        public string DepositId { get; set; } = null!;
       
        public int? TransNo { get; set; }
        public DateTime? TraDt { get; set; }
        public string? Descr { get; set; }
        [Key]
        public decimal Amount { get; set; }
        [Key]
        public string? Type { get; set; }
        public int? AccountNo { get; set; }
        public int? ContraNo { get; set; }
        public DateTime? ValueDt { get; set; }
        [Key]
        public int? TransId { get; set; }
        public int? VouchId { get; set; }
    }
}
