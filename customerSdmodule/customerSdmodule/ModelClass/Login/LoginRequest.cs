namespace customerSdmodule.ModelClass.Login
{
    public class LoginRequest : Request
    {

        public LoginRequest()
        {
            LoginRequest._Requesttype = "LoginRequest";
        }

        public LoginApI Data { get => (LoginApI)base.Data; set => base.Data = (LoginApI)value; }
    }

}
