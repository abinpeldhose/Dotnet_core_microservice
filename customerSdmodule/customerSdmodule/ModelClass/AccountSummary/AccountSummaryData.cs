namespace customerSdmodule.ModelClass.AccountSummary
{
    public class AccountSummaryData:BaseData
    {
        private string _customerid;
        private string _depositid;

        public string Customerid { get => _customerid; set => _customerid = value; }
        public string Depositid { get => _depositid; set => _depositid = value; }
    }
}
