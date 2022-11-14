namespace customerSdmodule.ModelClass.AccountDetails
{
    public class AccountdetailsRequest : Request
    {

        public AccountdetailsRequest()
        {
            AccountdetailsRequest._Requesttype = "AccountDetailsRequest";
        }

        public AccountdetailsApi Data { get => (AccountdetailsApi)base.Data; set => base.Data = (AccountdetailsApi)value; }
    }
}
