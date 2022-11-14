namespace customerSdmodule.ModelClass.WithdrawalTo
{
    public class WithdrawalToData:BaseData
    {
        private string _depositid;
        private string _usertype;

        public string Depositid { get => _depositid; set => _depositid = value; }
        public string Usertype { get => _usertype; set => _usertype = value; }
    }
}
