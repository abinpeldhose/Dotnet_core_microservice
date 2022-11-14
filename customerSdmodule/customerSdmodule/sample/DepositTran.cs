namespace customerSdmodule.sample
{
    public partial class DepositTran
    {
        public string DocId { get; set; } = null!;
        public int? TransNo { get; set; }
        public DateTime? TraDt { get; set; }
        public string? Descr { get; set; }
        public decimal Amount { get; set; }
        public string? Type { get; set; }
        public int? AccountNo { get; set; }
        public int? ContraNo { get; set; }
        public DateTime? ValueDt { get; set; }
        public int? TransId { get; set; }
        public int? VouchId { get; set; }
    }
}
