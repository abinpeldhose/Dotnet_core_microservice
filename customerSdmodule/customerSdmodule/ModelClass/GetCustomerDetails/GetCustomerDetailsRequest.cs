namespace customerSdmodule.ModelClass.GetCustomerDetails
{
    public class GetCustomerDetailsRequest :Request
    {
        public GetCustomerDetailsRequest()
        {
            GetCustomerDetailsRequest._Requesttype = "GetCustomerDetailsRequest";
        }

        public GetCustomerDetailsApi Data { get => (GetCustomerDetailsApi)base.Data; set => base.Data = (GetCustomerDetailsApi)value; }
    }
}
