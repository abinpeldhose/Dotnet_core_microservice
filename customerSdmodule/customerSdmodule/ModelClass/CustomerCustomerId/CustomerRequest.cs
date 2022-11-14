namespace customerSdmodule.ModelClass.CustomerCustomerId
{
    public class CustomerRequest : Request
    {

        public CustomerRequest()
        {
            CustomerRequest._Requesttype = "CustomercustIdrequest";
        }

        public CustomerApi Data { get => (CustomerApi)base.Data; set => base.Data = (CustomerApi)value; }
    }
}
