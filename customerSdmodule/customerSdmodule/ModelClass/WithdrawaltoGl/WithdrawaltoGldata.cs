namespace customerSdmodule.ModelClass.WithdrawaltoGl
{
    public class WithdrawaltoGldata:BaseData
    {

        private string _pledgeno;
        private string _usertype;

      
        public string Usertype { get => _usertype; set => _usertype = value; }
        public string Pledgeno { get => _pledgeno; set => _pledgeno = value; }
    }
}
