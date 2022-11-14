using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class SdInterest
    {
        public byte? FirmId { get; set; }
        public short? BranchId { get; set; }
        public short SchemeId { get; set; }
        public byte? ModuleId { get; set; }
        public int? StatusId { get; set; }
        public decimal? IntRate { get; set; }
        public byte? PeriodFrom { get; set; }
        public byte? PeriodTo { get; set; }
        public decimal? PreRate { get; set; }
        public decimal? LoanAdv { get; set; }
        public decimal? LoanRate { get; set; }
        public decimal? Annualyield { get; set; }
        public decimal? CitizenRate { get; set; }
        public decimal? CitizenYield { get; set; }

        public virtual SdScheme? SdScheme { get; set; }
        public virtual StatusMaster? Status { get; set; }
    }
}
