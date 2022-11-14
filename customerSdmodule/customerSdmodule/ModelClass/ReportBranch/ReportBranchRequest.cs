namespace customerSdmodule.ModelClass.ReportBranch
{
    public class ReportBranchRequest : Request
    {

        public ReportBranchRequest()
        {
            ReportBranchRequest._Requesttype = "ReportBranchRequest";
        }

        public ReportBranchApi Data { get => (ReportBranchApi)base.Data; set => base.Data = (ReportBranchApi)value; }
    }
}
