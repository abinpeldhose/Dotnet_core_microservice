namespace customerSdmodule.ModelClass.ReportBranch
{
    public class ReportBranchData:BaseData
    {
        private int _firmid;
        private int _branchid;
        private int _page;
        private int _pagesize;

        public int Firmid { get => _firmid; set => _firmid = value; }
        public int Branchid { get => _branchid; set => _branchid = value; }
        public int Page { get => _page; set => _page = value; }
        public int Pagesize { get => _pagesize; set => _pagesize = value; }
       
    }
}
