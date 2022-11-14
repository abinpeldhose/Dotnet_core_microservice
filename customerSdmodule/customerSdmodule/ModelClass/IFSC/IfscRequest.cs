namespace customerSdmodule.ModelClass.IFSC
{
    public class IfscRequest : Request
    {

        public IfscRequest()
        {
            IfscRequest._Requesttype = "IfscRequest";
        }

        public IfscApi Data { get => (IfscApi)base.Data; set => base.Data = (IfscApi)value; }
    }
}
