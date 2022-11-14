using System;
using System.Collections.Generic;

namespace customerSdmodule.sample
{
    public partial class TransidDetail
    {
        public byte FirmId { get; set; }
        public byte BranchId { get; set; }
        public int? CashPay { get; set; }
        public int? CashReceipt { get; set; }
        public int? TransferPay { get; set; }
        public int? TransferReceipt { get; set; }
        public int? Transno { get; set; }
        public int? VouchId { get; set; }
        public int? SubsidaryPay { get; set; }
        public int? SubsidaryReceipt { get; set; }
        public int? CustId { get; set; }
        public int? OtherCustId { get; set; }
        public int? CashPay1 { get; set; }
        public int? CashReceipt1 { get; set; }
        public int? STransid { get; set; }
    }
}
