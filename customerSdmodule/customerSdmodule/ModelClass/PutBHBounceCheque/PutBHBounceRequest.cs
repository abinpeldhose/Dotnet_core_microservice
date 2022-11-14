namespace customerSdmodule.ModelClass.PutBHBounceCheque
{
    public class PutBHBounceRequest:Request
    {
        public PutBHBounceRequest()
        {
            PutBHBounceRequest._Requesttype = "PutBHBounceRequest";
        }

        public PutBHBounceApi Data { get => (PutBHBounceApi)base.Data; set => base.Data = (PutBHBounceApi)value; }
    }
}
