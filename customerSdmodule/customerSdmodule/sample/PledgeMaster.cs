namespace customerSdmodule.sample
{

    public partial class PledgeMaster
    {
        public byte BranchId { get; set; }
        public byte FirmId { get; set; }
        public string PledgeNo { get; set; } = null!;
        public byte? SchemeId { get; set; }
        public string CustId { get; set; } = null!;
        public DateTime? TraDt { get; set; }
        public DateTime? MaturityDt { get; set; }
        public decimal ActWeight { get; set; }
        public decimal NetWeight { get; set; }
        public decimal? ApxVal { get; set; }
        public decimal LndRate { get; set; }
        public decimal PledgeVal { get; set; }
        public decimal IntRate { get; set; }
        public decimal SerRate { get; set; }
        public decimal AppRate { get; set; }
        public string? SchemeNm { get; set; }
        public string? CustName { get; set; }
        public short Period { get; set; }
        public decimal? OvrDue { get; set; }
        public bool? Counter { get; set; }
        public bool? EnhancementId { get; set; }
        public decimal? Balance { get; set; }
        public decimal? IntAcrud { get; set; }
        public byte? WbStatus { get; set; }
        public DateTime? Tradate { get; set; }
        public decimal? StoneWeight { get; set; }
        public decimal? CollateralValue { get; set; }
    }
}
