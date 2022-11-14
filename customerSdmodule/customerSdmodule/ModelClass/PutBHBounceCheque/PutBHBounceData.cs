namespace customerSdmodule.ModelClass.PutBHBounceCheque
{
    public class PutBHBounceData:BaseData
    {
        private string _chequeno;
        private string _depositid;
        private int _employeeid;
        private string _rejectreason;

        public string Chequeno { get => _chequeno; set => _chequeno = value; }
        public string DepositId { get => _depositid; set => _depositid = value; }
        public int EmpId { get => _employeeid; set => _employeeid = value; }
        public string RejectReason { get => _rejectreason; set => _rejectreason = value; }
    }
}
