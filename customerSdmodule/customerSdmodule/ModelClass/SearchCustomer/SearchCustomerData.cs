namespace customerSdmodule.ModelClass.SearchCustomer
{
    public class SearchCustomerData:BaseData
    {
        private string _search;
        private string _type;
        private int _page;
        private int _pagesize;

        public string Search { get => _search; set => _search = value; }
        public string Type { get => _type; set => _type = value; }
        public int Page { get => _page; set => _page = value; }
        public int Pagesize { get => _pagesize; set => _pagesize = value; }
    }
}
