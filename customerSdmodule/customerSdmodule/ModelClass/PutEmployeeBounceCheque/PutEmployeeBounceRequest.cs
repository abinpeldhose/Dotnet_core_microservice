namespace customerSdmodule.ModelClass.PutEmployeeBounceCheque
{
    public class PutEmployeeBounceRequest:Request
    {
        public PutEmployeeBounceRequest()
        {
            PutEmployeeBounceRequest._Requesttype = "PutEmployeeBounceRequest";
        }

        public PutEmployeeBounceApi Data { get => (PutEmployeeBounceApi)base.Data; set => base.Data = (PutEmployeeBounceApi)value; }
    }
}
