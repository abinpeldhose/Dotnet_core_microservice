namespace customerSdmodule.ModelClass.SubsidaryBank
{
    public class SubsidaryBankData:BaseData
    {
        private int _branchid;
        private int _firmid;
        private string _modeoftransaction;

        public int Branchid { get => _branchid; set => _branchid = value; }
        public int Firmid { get => _firmid; set => _firmid = value; }
        public string Modeoftransaction { get => _modeoftransaction; set => _modeoftransaction = value; }
    }
}
