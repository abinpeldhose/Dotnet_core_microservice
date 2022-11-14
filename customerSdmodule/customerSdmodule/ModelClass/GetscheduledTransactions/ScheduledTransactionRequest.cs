namespace customerSdmodule.ModelClass.GetscheduledTransactions
{
    public class ScheduledTransactionRequest : Request
    {
        public ScheduledTransactionRequest()
        {
            ScheduledTransactionRequest._Requesttype = "ScheduledTransactionRequest";
        }

        public ScheduledtransactionApi Data { get => (ScheduledtransactionApi)base.Data; set => base.Data = (ScheduledtransactionApi)value; }
    }
}
