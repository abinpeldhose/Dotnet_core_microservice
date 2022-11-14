namespace customerSdmodule.ModelClass.AgentEmployee
{
    public class AgentEmployeeData:BaseData
    {
        private string _search;
        private string _type;

        public string Search { get => _search; set => _search = value; }
        public string Type { get => _type; set => _type = value; }
    }
}
