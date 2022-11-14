namespace customerSdmodule.ModelClass.Registration
{
    public class RegistartionData:BaseData
    {
        private int _firmid;
        private int _branchid;
        private string _customerid;
        private string _userid;
        private string _password;
        private string _Phone;

        public int Firmid { get => _firmid; set => _firmid = value; }
        public int Branchid { get => _branchid; set => _branchid = value; }
        public string Customerid { get => _customerid; set => _customerid = value; }
        public string Userid { get => _userid; set => _userid = value; }
        public string Password { get => _password; set => _password = value; }
        public string Phone { get => _Phone; set => _Phone = value; }
    }
}
