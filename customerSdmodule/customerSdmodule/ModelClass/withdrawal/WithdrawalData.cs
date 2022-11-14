namespace customerSdmodule.ModelClass.withdrawal
{
    public class withdrawalData:BaseData
    {
        private byte _firmId;
        private short _branchId;
        private byte _moduleId;
        private string _depositId;
        private string _startDate;
        private string _endDate;
        private int _noOfOccurence;
        private string _frequency;
        private decimal _amount;
        private string _ifsc;
        private string _transactionMethod;
        private string _accountNumber;
        private int userType;
        private string _userId;
        public string tfr_data { get; set; }
        public string statusAppWeb { get; set; }


        public string phoneNo { get; set; }
        public string tfrsdno { get; set; }
        public string tframt { get; set; }
        public string odint { get; set; }
        public string currinstno { get; set; }

        public string Plgno { get; set; }

        public byte FirmId { get => _firmId; set => _firmId = value; }


        public short BranchId { get => _branchId; set => _branchId = value; }
        public byte ModuleId { get => _moduleId; set => _moduleId = value; }
        public string DepositId { get => _depositId; set => _depositId = value; }
        public string StartDate { get => _startDate; set => _startDate = value; }
        public string EndDate { get => _endDate; set => _endDate = value; }
        public int NoOfOccurence { get => _noOfOccurence; set => _noOfOccurence = value; }
        public string Frequency { get => _frequency; set => _frequency = value; }
        public decimal Amount { get => _amount; set => _amount = value; }
        public string Ifsc { get => _ifsc; set => _ifsc = value; }
        public string TransactionMethod { get => _transactionMethod; set => _transactionMethod = value; }
        public string AccountNumber { get => _accountNumber; set => _accountNumber = value; }
        public int UserType { get => userType; set => userType = value; }
        public string UserId { get => _userId; set => _userId = value; }
        public long SubsidiaryBankAccountno { get => _subsidiaryBankAccountno; set => _subsidiaryBankAccountno = value; }
        public string ChequNo { get => _chequNo; set => _chequNo = value; }
        public string CustomerBank { get => _customerBank; set => _customerBank = value; }
        public string SubsidiaryBankName { get => _subsidiaryBankName; set => _subsidiaryBankName = value; }
        public string RealizationDate { get => _realizationDate; set => _realizationDate = value; }
        public int EmployeeCode { get => _employeeCode; set => _employeeCode = value; }
        public string CustomerName { get => _customerName; set => _customerName = value; }
        public int BranchBankid { get => _branchBankid; set => _branchBankid = value; }
        public string Customerid { get => _customerid; set => _customerid = value; }

        private long _subsidiaryBankAccountno;
        private string _chequNo;
        private string _customerBank;
        private string _subsidiaryBankName;
        private string _realizationDate;
        private int _employeeCode;
        private string _customerName;
        private int _branchBankid;
        private string _customerid;
    }

    // byte firmId, short branchId, byte? moduleId, string depositId,
    //  DateTime startDate, DateTime closeDate, int noOfOccurence, string? frquency, decimal amount, string? ifsc, String transactionMethod,
    // string? accountNumber, int? userType, string? userId
}
