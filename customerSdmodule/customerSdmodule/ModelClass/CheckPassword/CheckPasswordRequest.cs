namespace customerSdmodule.ModelClass.CheckPassword
{
    public class CheckPasswordRequest : Request
    {

        public CheckPasswordRequest()
        {
            CheckPasswordRequest._Requesttype = "CheckPasswordRequest";
        }

        public CheckPasswordAPI Data
        {
            get => (CheckPasswordAPI)base.Data; set => base.Data = (CheckPasswordAPI)value;

        }

    }
}
