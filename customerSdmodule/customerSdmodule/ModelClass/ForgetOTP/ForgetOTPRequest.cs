namespace customerSdmodule.ModelClass.forgetpassword
{
    public class ForgetOTPRequest:Request
    {
        public ForgetOTPRequest()
        {
            ForgetOTPRequest._Requesttype = "forgetpassowordrequest";
        }

        public ForgetOTPAPI passwordData { get => (ForgetOTPAPI)base.Data; set => base.Data = (ForgetOTPAPI)value; }
    }
}
