namespace customerSdmodule.ModelClass.StatementDetails
{
    public class StatementDetailsRequest:Request
    {
        public StatementDetailsRequest()
        {
            StatementDetailsRequest._Requesttype = "StatementDetailsRequest";
        }
        public StatementDetailsApi Data { get => (StatementDetailsApi)base.Data; set => base.Data = (StatementDetailsApi)value; }

    }
}
