namespace customerSdmodule.ModelClass.CustomerPhoneNumberForget
{
    public class CustomerPhoneNumberRequest : Request
    {
        public CustomerPhoneNumberRequest()
        {
            CustomerPhoneNumberRequest._Requesttype = "CustomerPhoneNumberForgetRequest";
        }

        public CustomerPhoneNumberForgetApi Data { get => (CustomerPhoneNumberForgetApi)base.Data; set => base.Data = (CustomerPhoneNumberForgetApi)value; }
    }
}
