namespace customerSdmodule.ModelClass.LoginMpin
{
    public class LoginMpinData:BaseData
    {
        private string _mpin;
        private string _devicetoken;

        public string mpin { get => _mpin; set => _mpin = value; }
        public string deviceToken { get => _devicetoken; set => _devicetoken = value; }
    }
}
