namespace customerSdmodule.ModelClass.AccountSummary
{
    public class AccountSummaryRequest : Request
    {

        public AccountSummaryRequest()
        {
            AccountSummaryRequest._Requesttype = "AccountSummaryRequest";
        }

        public AccountSummaryApi Data { get => (AccountSummaryApi)base.Data; set => base.Data = (AccountSummaryApi)value; }
    }
}
