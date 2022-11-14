namespace customerSdmodule.ModelClass.Deposit
{
    public class DepositRequest : Request
    {

        public DepositRequest()
        {
            DepositRequest._Requesttype = "DepositRequest";
        }
        
        public DepositApi Data { get => (DepositApi)base.Data; set => base.Data = (DepositApi)value; }
    }
}
