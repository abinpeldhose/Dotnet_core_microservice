namespace customerSdmodule.ModelClass.AgentEmployee
{
    public class AgentEmployeeRequest : Request
    {
        public AgentEmployeeRequest()
        {
            AgentEmployeeRequest._Requesttype = "AgentEmployeeSearchRequest";
        }

        public AgentEmployeeApi Data { get => (AgentEmployeeApi)base.Data; set => base.Data = (AgentEmployeeApi)value; }
    }
}
