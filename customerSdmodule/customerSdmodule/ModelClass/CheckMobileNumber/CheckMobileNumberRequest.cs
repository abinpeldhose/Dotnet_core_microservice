namespace customerSdmodule.ModelClass.CheckMobileNumber
{
    public class CheckMobileNumberRequest:Request
    {
        public CheckMobileNumberRequest()
        {
            CheckMobileNumberRequest._Requesttype = "CheckMobileNumberRequest";
        }

        public CheckMobileNumberAPI Data { get => (CheckMobileNumberAPI)base.Data; set => base.Data = (CheckMobileNumberAPI)value; }
    }
}
