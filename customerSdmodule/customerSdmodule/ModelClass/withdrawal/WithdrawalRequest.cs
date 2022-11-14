namespace customerSdmodule.ModelClass.withdrawal
{
    public class WithdrawalRequest:Request
    {
        public WithdrawalRequest()
        {
            WithdrawalRequest._Requesttype = "WithdrawalRequest";
        }

        public WithdrawalAPI Withdrawal { get => (WithdrawalAPI)base.Data; set => base.Data = (WithdrawalAPI)value; }
    }

 
}
