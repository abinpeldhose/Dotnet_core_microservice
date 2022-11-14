namespace customerSdmodule.ModelClass.VerifyOtp
{
    public class VerifyOtpRequest : Request
    {
        public VerifyOtpRequest()
        {
            VerifyOtpRequest._Requesttype = "VerifyOTPRequest";
        }

        public VerifyOtpApi Data { get => (VerifyOtpApi)base.Data; set => base.Data = (VerifyOtpApi)value; }
    }
}
