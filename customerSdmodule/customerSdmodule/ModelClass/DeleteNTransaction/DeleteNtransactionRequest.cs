namespace customerSdmodule.ModelClass.DeleteNTransaction
{
    public class DeleteNtransactionRequest:Request
    {
        public DeleteNtransactionRequest()
        {
            DeleteNtransactionRequest._Requesttype = "DeleteNtransactionRequest";
        }

        public DeleteNTransactionApi Data { get => (DeleteNTransactionApi)base.Data; set => base.Data = (DeleteNTransactionApi)value; }
    }
}
