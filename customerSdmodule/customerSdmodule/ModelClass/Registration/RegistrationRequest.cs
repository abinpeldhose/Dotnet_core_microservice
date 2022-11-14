namespace customerSdmodule.ModelClass.Registration
{
    public class RegistrationRequest : Request
    {
        public RegistrationRequest()
        {
            RegistrationRequest._Requesttype = "PostRegistrationrequest";
        }

        public RegistrationApi Data { get => (RegistrationApi)base.Data; set => base.Data = (RegistrationApi)value; }
    }
}
