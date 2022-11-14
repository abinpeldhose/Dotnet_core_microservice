namespace customerSdmodule.ModelClass.DeleteNTransaction
{
    public class DeleteNTransactionData:BaseData
    {
        private int _flag;
        private decimal _rtid;
        private DateTime _transactionDate;
        private string _userType;

        public int flag { get => _flag; set => _flag = value; }
        public decimal rtId { get => _rtid; set => _rtid = value; }
        public DateTime transactionDate { get => _transactionDate; set => _transactionDate = value; }
        public string userType { get => _userType; set => _userType = value; }
    }
}
