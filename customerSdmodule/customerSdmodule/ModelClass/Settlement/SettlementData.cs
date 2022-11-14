namespace customerSdmodule.ModelClass.Settlement
{
    public class SettlementData:BaseData
    {
        private string _customerId;
        private string _accountNumber;
        private string _transactiontype;

        private short _branchId;
        private byte _firmId;
        private int _branchBankid;
        private string _chequeNo;
        private string _customerBank;
        private string _subsidiaryBankName;
        private long _subsidiaryBankAccountno;
        private int _employeeCode;
        private string _customerName;
        private string _realizationDate;


        public string CustomerId { get => _customerId; set => _customerId = value; }
        public string AccountNumber { get => _accountNumber; set => _accountNumber = value; }
        public string Transactiontype { get => _transactiontype; set => _transactiontype = value; }
        public short BranchId { get => _branchId; set => _branchId = value; }
        public byte FirmId { get => _firmId; set => _firmId = value; }
        public int BranchBankid { get => _branchBankid; set => _branchBankid = value; }
        public string ChequeNo { get => _chequeNo; set => _chequeNo = value; }
        public string CustomerBank { get => _customerBank; set => _customerBank = value; }
        public string SubsidiaryBankName { get => _subsidiaryBankName; set => _subsidiaryBankName = value; }
        public long SubsidiaryBankAccountno { get => _subsidiaryBankAccountno; set => _subsidiaryBankAccountno = value; }
        public int EmployeeCode { get => _employeeCode; set => _employeeCode = value; }
        public string CustomerName { get => _customerName; set => _customerName = value; }
        public string RealizationDate { get => _realizationDate; set => _realizationDate = value; }
    }
}
