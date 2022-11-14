namespace customerSdmodule.ModelClass.ReportCompany
{
    public class ReportCompanyData:BaseData
    {
        private int _firmid;
        private int _flag;
        private int _page;
        private int _pagesize;

        public int Firmid { get => _firmid; set => _firmid = value; }
        public int Flag { get => _flag; set => _flag = value; }
        public int Page { get => _page; set => _page = value; }
        public int Pagesize { get => _pagesize; set => _pagesize = value; }
    }
}
