namespace customerSdmodule.ModelClass.ForgetPassword
{
    public class ForgetPasswordRequest:Request
    {
        public ForgetPasswordRequest()
        {
            ForgetPasswordRequest._Requesttype = "forgetpassowordrequest";
        }

        public ForgetPasswordAPI Data { get => (ForgetPasswordAPI)base.Data; set => base.Data = (ForgetPasswordAPI)value; }
    }
}
