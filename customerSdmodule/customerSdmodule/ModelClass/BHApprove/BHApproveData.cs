namespace customerSdmodule.ModelClass.BHApprove
{
    public class BHApproveData:BaseData
    {
        private int _firmId;
        private int _branchId;
        private int _moduleId;
        private string _depositId;
        private int _bhId;
        private string _chequeNo;
        private string _chequeClearDate;

        public int FirmId { get => _firmId; set => _firmId = value; }
        public int BranchId { get => _branchId; set => _branchId = value; }
        public int ModuleId { get => _moduleId; set => _moduleId = value; }
        public string DepositId { get => _depositId; set => _depositId = value; }
        public int BhId { get => _bhId; set => _bhId = value; }
        public string ChequeNo { get => _chequeNo; set => _chequeNo = value; }
        public string  ChequeClearDate { get => _chequeClearDate; set => _chequeClearDate = value; }
    }
}
