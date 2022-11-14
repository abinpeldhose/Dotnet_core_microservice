namespace customerSdmodule.ModelClass.WithdrawaltoGl
{
    public class WithdrawaltoGlRequest : Request
    {

        public WithdrawaltoGlRequest()
        {
            WithdrawaltoGlRequest._Requesttype = "RecurringRequest";
        }

        public WithdrwaltoGlApi Data { get => (WithdrwaltoGlApi)base.Data; set => base.Data = (WithdrwaltoGlApi)value; }
    }
}
