using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class SdVerification
    {
        public byte FirmId { get; set; }
        public short BranchId { get; set; }
        public byte ModuleId { get; set; }
        public string VerifyId { get; set; } = null!;
        public string? DepositId { get; set; }
        public string CustId { get; set; } = null!;
        public string? CustName { get; set; }
        public string? DepositType { get; set; }
        public decimal? DepositAmt { get; set; }
        public decimal? IntRt { get; set; }
        public DateTime? DepositDate { get; set; }
        public int? SchemeId { get; set; }
        public decimal? Mobilizer { get; set; }
        public string? Nominee { get; set; }
        public string? Minor { get; set; }
        public int? IncentiveId { get; set; }
        public bool? Chqstatus { get; set; }
        public int? CategoryId { get; set; }
        public string? RejectId { get; set; }
        public string? AbhStatus { get; set; }
        public int? AbhId { get; set; }
        public string? BhStatus { get; set; }
        public int? BhId { get; set; }
        public string UserId { get; set; } = null!;
        public byte? ProcessPeriod { get; set; }
        public string? RejectReason { get; set; }
        public bool? BranchCounter { get; set; }
        public byte? StatusAppweb { get; set; }
        public int? TdsCode { get; set; }

        public virtual BranchMaster Branch { get; set; } = null!;
        public virtual Customer Cust { get; set; } = null!;
        public virtual DepositTypeMaster? DepositTypeNavigation { get; set; }
        public virtual FirmMaster Firm { get; set; } = null!;
    }
}
