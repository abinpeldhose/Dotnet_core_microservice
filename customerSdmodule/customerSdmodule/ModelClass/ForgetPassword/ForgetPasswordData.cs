namespace customerSdmodule.ModelClass.ForgetPassword
{
    public class ForgetPasswordData:BaseData
    {
        private string _mobilenumber;
        private string _newpassword;
        private string _customerid;

        public string Mobilenumber { get => _mobilenumber; set => _mobilenumber = value; }
        public string Newpassword { get => _newpassword; set => _newpassword = value; }
        public string Customerid { get => _customerid; set => _customerid = value; }
    }
}
