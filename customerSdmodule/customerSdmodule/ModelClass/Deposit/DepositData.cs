namespace customerSdmodule.ModelClass.Deposit
{
    public class DepositData:BaseData
    {

        private string _depositId;
        private short _branchId;
        private byte _firmId;
        private int _branchBankid;
        private string _customerId;
        private decimal _amount;
        private string _transactionMethod;
        private int userType;
        private string _chequNo;
        private string _customerBank;
        private string _subsidiaryBankName;
        private long _subsidiaryBankAccountno;
        private int _employeeCode;
        private string _customerName;
        private string _realizationDate;

        public string DepositId { get => _depositId; set => _depositId = value; }
        public short BranchId { get => _branchId; set => _branchId = value; }
        public byte FirmId { get => _firmId; set => _firmId = value; }
        public decimal Amount { get => _amount; set => _amount = value; }
        public string TransactionMethod { get => _transactionMethod; set => _transactionMethod = value; }
        public int UserType { get => userType; set => userType = value; }
        public string ChequeNo { get => _chequNo; set => _chequNo = value; }
        public string CustomerBank { get => _customerBank; set => _customerBank = value; }
        public string SubsidiaryBankName { get => _subsidiaryBankName; set => _subsidiaryBankName = value; }
        public long SubsidiaryBankAccountno { get => _subsidiaryBankAccountno; set => _subsidiaryBankAccountno = value; }
        public int EmployeeCode { get => _employeeCode; set => _employeeCode = value; }
        public string CustomerName { get => _customerName; set => _customerName = value; }
        public string RealizationDate { get => _realizationDate; set => _realizationDate = value; }
        public string CustomerId { get => _customerId; set => _customerId = value; }
        public int BranchBankid { get => _branchBankid; set => _branchBankid = value; }
    }
}
