using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class PaymentgatewayMaster
    {
        public String? UserType { get; set; }
        public string? PaymentgatewayName { get; set; }
        public string ProviderId { get; set; } = null!;
        public string? PaymentgatewayType { get; set; }
        public string? PaymentType { get; set; }
        public string? CommissionFlat { get; set; }
        public decimal? PaymentgatewayCommission { get; set; }
        public string? ComissionflatDescription { get; set; }
    }
}
