namespace customerSdmodule.ModelClass.CustomerToAccounts
{
    public class CustomertoAccountsRequest : Request
    {
        public CustomertoAccountsRequest()
        {
            CustomertoAccountsRequest._Requesttype = "CustomerToAccountsRequest";
        }

        public CustomerToAccountsApi Data { get => (CustomerToAccountsApi)base.Data; set => base.Data = (CustomerToAccountsApi)value; }
    }
}
