namespace customerSdmodule.ModelClass.GetCustomerPhone
{
    public class GetCustomerPhoneRequest:Request
    {
        public GetCustomerPhoneRequest()
        {
            GetCustomerPhoneRequest._Requesttype = "GetCustomerPhoneRequest";
        }

        public GetCustomerPhoneApi Data { get => (GetCustomerPhoneApi)base.Data; set => base.Data = (GetCustomerPhoneApi)value; }
    }
}
