namespace customerSdmodule.ModelClass.CustomerAcccounts
{
    public class CustomerAccountsRequest : Request
    {
        public CustomerAccountsRequest()
        {
            CustomerAccountsRequest._Requesttype = "CustomerAccountsRequest";
        }

        public CustomerAccountsAPi Data { get => (CustomerAccountsAPi)base.Data; set => base.Data = (CustomerAccountsAPi)value; }
    }
}
