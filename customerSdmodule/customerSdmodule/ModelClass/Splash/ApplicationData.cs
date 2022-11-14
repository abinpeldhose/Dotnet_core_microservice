namespace customerSdmodule.ModelClass.Splash
{
    public class ApplicationData:BaseData
    {
        private int _applicationNumber;
        private string deviceToken;

        public int ApplicationNumber { get => _applicationNumber; set => _applicationNumber = value; }
        public string DeviceToken { get => deviceToken; set => deviceToken = value; }
    }
}
