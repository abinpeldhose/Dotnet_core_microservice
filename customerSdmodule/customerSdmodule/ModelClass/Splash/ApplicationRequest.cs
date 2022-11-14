namespace customerSdmodule.ModelClass.Splash
{
    public class ApplicationRequest:Request
    {
        public ApplicationRequest()
        {
            ApplicationRequest._Requesttype = "splashrequest";
        }

        public AppicationAPI ApplicationData { get => (AppicationAPI)base.Data; set => base.Data = (AppicationAPI)value; }

    }
}
