using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class PaymentgatewayDescription
    {
        public string ProviderId { get; set; } = null!;
        public string? PaymentFlat { get; set; }
        public string? PaymentDescription { get; set; }
    }
}
