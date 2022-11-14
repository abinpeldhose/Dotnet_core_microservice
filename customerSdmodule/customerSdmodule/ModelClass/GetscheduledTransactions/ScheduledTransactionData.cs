namespace customerSdmodule.ModelClass.GetscheduledTransactions
{
    public class ScheduledTransactionData:BaseData
    {
        private string _customerid;

        public string CustomerID { get => _customerid; set => _customerid = value; }
    }
}
