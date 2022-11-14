using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class SdMaster
    {
        public byte FirmId { get; set; }
        public short BranchId { get; set; }
        public byte ModuleId { get; set; }
        public string DepositId { get; set; } = null!;
        public string CustId { get; set; } = null!;
        public string CustName { get; set; } = null!;
        public string DepositType { get; set; } = null!;
        public decimal DepositAmt { get; set; }
        public decimal IntRt { get; set; }
        public DateTime DepositDate { get; set; }
        public DateTime? TrancationDate { get; set; }
        public DateTime CloseDate { get; set; }
        public int? SchemeId { get; set; }
        public int? StatusId { get; set; }
        public int? Nominee { get; set; }
        public int TdsCode { get; set; }
        public int? Minor { get; set; }
        public int? IncentiveId { get; set; }
        public string? Citizen { get; set; }
        public decimal? Balance { get; set; }
        public int? Chqstatus { get; set; }
        public decimal? FinInterest { get; set; }
        public bool? CategoryId { get; set; }
        public decimal? Mobilizer { get; set; }
        public bool? IntTfrType { get; set; }
        public string? Lien { get; set; }

        public virtual BranchMaster Branch { get; set; } = null!;
        public virtual CatrgoryMaster? Category { get; set; }
        public virtual StatusMaster? ChqstatusNavigation { get; set; }
        public virtual CitizenMaster? CitizenNavigation { get; set; }
        public virtual Customer Cust { get; set; } = null!;
        public virtual DepositTypeMaster DepositTypeNavigation { get; set; } = null!;
        public virtual FirmMaster Firm { get; set; } = null!;
        public virtual StatusMaster? Incentive { get; set; }
        public virtual StatusMaster? MinorNavigation { get; set; }
        public virtual StatusMaster? NomineeNavigation { get; set; }
        public virtual StatusMaster? Status { get; set; }
    }


}
