using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class PaymentgatewayTran
    {
        public byte FirmId { get; set; }
        public short BranchId { get; set; }
        public string UniqueId { get; set; } = null!;
        public DateTime? TraDt { get; set; }
        public string? BankResponse { get; set; }
        public decimal Amount { get; set; }
    }
}
