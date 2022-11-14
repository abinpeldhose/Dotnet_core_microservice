namespace customerSdmodule.ModelClass.CustomerAcccounts
{
    public class CustomerAccountsData:BaseData
    {
        private string _customerid;
        private string _depositid;
        private string _id;


        public string Customerid { get => _customerid; set => _customerid = value; }
        public string Depositid { get => _depositid; set => _depositid = value; }
        public string Id { get => _id; set => _id = value; }
    }
}
