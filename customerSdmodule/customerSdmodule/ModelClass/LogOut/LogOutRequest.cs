namespace customerSdmodule.ModelClass.LogOut
{
    public class LogOutRequest:Request
    {
        public LogOutRequest()
        {
            LogOutRequest._Requesttype = "LogOutRequest";
        }
        public LogOutAPI Data { get => (LogOutAPI)base.Data; set => base.Data = (LogOutAPI)value; }
    }
}
