using System.ComponentModel.DataAnnotations;

namespace customerSdmodule.Model1
{
    public partial class SdScheduledTranMaster
    {
        public byte? FirmId { get; set; }
        public short? BranchId { get; set; }
        public string? DepositId { get; set; }
        public DateTime? TraDt { get; set; }
        public string? Descr { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public byte? NoTransactions { get; set; }
        public DateTime? NextTransaction { get; set; }
        public decimal? Amount { get; set; }
        [Key]
        public string? TraType { get; set; }
        public string? RecurringType { get; set; }
        public byte? Status { get; set; }
    }
}
