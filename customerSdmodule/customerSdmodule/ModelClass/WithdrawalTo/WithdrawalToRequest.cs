namespace customerSdmodule.ModelClass.WithdrawalTo
{
    public class WithdrawalToRequest : Request
    {
        public WithdrawalToRequest()
        {
            WithdrawalToRequest._Requesttype = "WithdrawalToRequest";
        }

        public WithdrawalToApi Data { get => (WithdrawalToApi)base.Data; set => base.Data = (WithdrawalToApi)value; }
    }
}
