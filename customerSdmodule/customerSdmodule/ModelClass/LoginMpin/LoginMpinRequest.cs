namespace customerSdmodule.ModelClass.LoginMpin
{
    public class LoginMpinRequest:Request
    {
        public LoginMpinRequest()
        {
            LoginMpinRequest._Requesttype = "LoginMpinRequest";
        }

        public LoginMpinApi Data { get => (LoginMpinApi)base.Data; set => base.Data = (LoginMpinApi)value; }
    }
}
