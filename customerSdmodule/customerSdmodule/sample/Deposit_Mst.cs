namespace customerSdmodule.sample
{
    public partial class DepositMst
    {
        public byte FirmId { get; set; }
        public byte BranchId { get; set; }
        public byte ModuleId { get; set; }
        public string DocId { get; set; } = null!;
        public string CustId { get; set; } = null!;
        public string CustName { get; set; } = null!;
        public string DepType { get; set; } = null!;
        public byte? DepPrd { get; set; }
        public decimal DepAmt { get; set; }
        public decimal IntRt { get; set; }
        public DateTime DepDt { get; set; }
        public DateTime? TraDt { get; set; }
        public DateTime? DueDt { get; set; }
        public DateTime ClsDt { get; set; }
        public byte? SchemeId { get; set; }
        public decimal? MatVal { get; set; }
        public int? EmpId { get; set; }
        public byte? IntTfrType { get; set; }
        public string? Renew { get; set; }
        public int? StatusId { get; set; }
        public string? Nominee { get; set; }
        public string? Lean { get; set; }
        public string? DuplFlag { get; set; }
        public bool TdsCode { get; set; }
        public string? Minor { get; set; }
        public byte? ProcessPrd { get; set; }
        public byte? InstNo { get; set; }
        public decimal? IntAcrued { get; set; }
        public short? LockPrd { get; set; }
        public bool? MobFlg { get; set; }
        public string? LetterFlag { get; set; }
        public byte? CBranch { get; set; }
        public string IntimationId { get; set; } = null!;
        public string? Citizen { get; set; }
        public decimal? Balance { get; set; }
        public decimal? PreRate { get; set; }
        public byte TdsStatus { get; set; }
        public bool? Chqstatus { get; set; }
        public short? IrBranch { get; set; }
        public decimal? FinInterest { get; set; }
        public bool? SpecialCategory { get; set; }
    }
}
