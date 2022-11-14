namespace customerSdmodule.ModelClass.ReportCompany
{
    public class ReportCompanyRequest : Request
    {

        public ReportCompanyRequest()
        {
            ReportCompanyRequest._Requesttype = "ReportCompanyRequest";
        }

        public ReportCompanyApi Data { get => (ReportCompanyApi)base.Data; set => base.Data = (ReportCompanyApi)value; }
    }
}
