namespace customerSdmodule.ModelClass.SubsidaryBank
{
    public class SubsidaryBankRequest :Request
    {
        public SubsidaryBankRequest()
        {
            SubsidaryBankRequest._Requesttype = "SubsidaryBankRequest";
        }

        public SubsidaryBankApi Data { get => (SubsidaryBankApi)base.Data; set => base.Data = (SubsidaryBankApi)value; }
    }
}
