namespace customerSdmodule.sample
{
    public partial class PledgeStatus
    {
        public string? PledgeNo { get; set; }
        public DateTime? DueDt { get; set; }
        public DateTime? CloseDt { get; set; }
        public byte? StatusId { get; set; }
        public byte? AuctionId { get; set; }
        public byte? ReleaseId { get; set; }
        public DateTime? TfrDt { get; set; }
        public string? LetterStatus { get; set; }
        public string? SmsCode { get; set; }
        public bool? TfrFlg { get; set; }
        public byte? ClassificationId { get; set; }
        public DateTime? AuctionDt { get; set; }
        public DateTime? LastUptodate { get; set; }
        public byte? InterestFlag { get; set; }
        public byte? FestivalOffer { get; set; }
        public string? ShelfNo { get; set; }
        public string? DClassification { get; set; }
        public decimal? TareWeight { get; set; }
        public string? StickerNo { get; set; }
        public string? InventoryId { get; set; }
        public string? InventoryidTemp { get; set; }
    }
}
