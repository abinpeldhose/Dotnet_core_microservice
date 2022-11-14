namespace customerSdmodule.ModelClass.SetMpin
{
    public class SetMpinRequest : Request
    {
        public SetMpinRequest()
        {
            SetMpinRequest._Requesttype = "SetMpinRequest";
        }

        public SetMpinApi Data { get => (SetMpinApi)base.Data; set => base.Data = (SetMpinApi)value; }
    }
}
