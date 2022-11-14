namespace customerSdmodule.ModelClass.CustomerToAccounts
{
    public class CustomerToAccountsData:BaseData
    {
        private string _customerid;
        private string _usertype;

        public string Customerid { get => _customerid; set => _customerid = value; }
        public string Usertype { get => _usertype; set => _usertype = value; }
    }
}
