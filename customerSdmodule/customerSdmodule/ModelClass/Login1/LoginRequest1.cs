namespace customerSdmodule.ModelClass.Login1
{
    public class LoginRequest1 : Request
    {

        public LoginRequest1()
        {
            LoginRequest1._Requesttype = "LoginRequest";
        }

        public Login1 Data { get => (Login1)base.Data; set => base.Data = (Login1)value; }
    }
}
