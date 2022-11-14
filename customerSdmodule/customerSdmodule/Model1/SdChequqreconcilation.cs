using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class SdChequqreconcilation
    {
        public int SlNo { get; set; }
        public string? EmployeeName { get; set; }
        public string? CustomerName { get; set; }
        public DateTime? ChequesubmitDate { get; set; }
        public DateTime? EmployeeverifyDate { get; set; }
        public byte? StatusId { get; set; }
        public string? ChequeNumber { get; set; }
        public byte? FirmId { get; set; }
        public short? BranchId { get; set; }
        public int? EmpCode { get; set; }
        public string? Depositno { get; set; }
        public string? CustomerBank { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? RealizationDate { get; set; }

        public virtual SdStatusMaster? Status { get; set; }
    }
}
