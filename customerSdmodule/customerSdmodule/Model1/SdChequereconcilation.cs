using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace customerSdmodule.Model1
{
    public partial class SdChequereconcilation
    {
        public byte? FirmId { get; set; }
        public short? BranchId { get; set; }
        public string Chequeno { get; set; } = null!;
        public string? CustomerName { get; set; }
        public string? CustomerBank { get; set; }
        public DateTime? ChqSubmiteDate { get; set; }
        public decimal? Amount { get; set; }
        public string? DepositId { get; set; }
        public long? SubsidiarybankAccountno { get; set; }
        public string? SubsidiarybankName { get; set; }
        public int? EmployeeCode { get; set; }
        public DateTime? EmployeeVerifyDate { get; set; }
        public byte? StatusId { get; set; }
        public DateTime? RealizationDate { get; set; }
        public int? BhId { get; set; }
        public DateTime? BhVerifyDate { get; set; }
        public int? AbhId { get; set; }
        public DateTime? AbhVerifyDate { get; set; }
        public string? RejectReason { get; set; }
        public int? ChequeSeq { get; set; }
        public DateTime? ChequeCleardt { get; set; }
        public int? BranchbankId { get; set; }
    }
}
