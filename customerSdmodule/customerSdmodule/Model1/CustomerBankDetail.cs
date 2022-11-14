using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class CustomerBankDetail
    {
        public string? CustId { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string AccountNo { get; set; } = null!;
        public string? Ifsc { get; set; }
        public string? BankHolderName { get; set; }
    }
}
