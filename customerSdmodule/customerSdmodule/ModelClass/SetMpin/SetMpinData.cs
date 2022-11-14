namespace customerSdmodule.ModelClass.SetMpin
{
    public class SetMpinData:BaseData
    {
        private string _userid;
        private string _phone;
        private string _mpin;
        private string _imeinumber;
        private string _devicetoken;
        private string _smsrefid;

        public string Userid { get => _userid; set => _userid = value; }
        public string Phone { get => _phone; set => _phone = value; }
        public string Mpin { get => _mpin; set => _mpin = value; }
        public string Imeinumber { get => _imeinumber; set => _imeinumber = value; }
        public string Devicetoken { get => _devicetoken; set => _devicetoken = value; }
        public string Smsrefid { get => _smsrefid; set => _smsrefid = value; }
    }
}
