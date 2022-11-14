namespace customerSdmodule.ModelClass.RD
{
    public class RDRequest : Request
    {

        public RDRequest()
        {
            RDRequest._Requesttype = "RecurringRequest";
        }

        public RdApi Data { get => (RdApi)base.Data; set => base.Data = (RdApi)value; }
    }
}
