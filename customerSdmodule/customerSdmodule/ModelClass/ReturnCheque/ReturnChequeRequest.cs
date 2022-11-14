namespace customerSdmodule.ModelClass.ReturnCheque
{
    public class ReturnChequeRequest:Request
    {
        public ReturnChequeRequest()
        {
            ReturnChequeRequest._Requesttype = "ReturnChequeRequest";
        }

        public ReturnChequeApi Data { get => (ReturnChequeApi)base.Data; set => base.Data = (ReturnChequeApi)value; }
    }
}
