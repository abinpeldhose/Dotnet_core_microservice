namespace customerSdmodule.ModelClass.ChequeEmployeeVerify
{
    public class ChequeEmployeeVerifyData:BaseData
    {


        private string _depositid;
        private string _chequeno;
        private string _cleardate;

        public string Depositid { get => _depositid; set => _depositid = value; }
        public string chqNo { get => _chequeno; set => _chequeno = value; }
        public string ClearDate { get => _cleardate; set => _cleardate = value; }
    }
}
