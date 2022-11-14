using System;
using System.Collections.Generic;

namespace customerSdmodule.Sample1
{
    public partial class CustomerPhoto
    {
        public string CustId { get; set; } = null!;
        public byte[]? PledgePhoto { get; set; }
        public byte[]? KycPhoto { get; set; }
        public byte[]? Signature { get; set; }
    }
}
