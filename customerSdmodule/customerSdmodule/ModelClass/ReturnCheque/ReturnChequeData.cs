namespace customerSdmodule.ModelClass.ReturnCheque
{
    public class ReturnChequeData:BaseData
    {
        private string _depositid;
        private string _chequeno;

        public string Depositid { get => _depositid; set => _depositid = value; }
        public string chqNo { get => _chequeno; set => _chequeno = value; }
    }
}
