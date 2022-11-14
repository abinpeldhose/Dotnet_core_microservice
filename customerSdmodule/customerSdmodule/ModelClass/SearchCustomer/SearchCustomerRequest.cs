namespace customerSdmodule.ModelClass.SearchCustomer
{
    public class SearchCustomerRequest : Request
    {
        public SearchCustomerRequest()
        {
            SearchCustomerRequest._Requesttype = "SearchCustomerRequest";
        }

        public SearchCustomerApi Data { get => (SearchCustomerApi)base.Data; set => base.Data = (SearchCustomerApi)value; }
    }
}
