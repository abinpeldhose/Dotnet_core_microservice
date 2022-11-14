namespace customerSdmodule.ModelClass.StatementDetails
{
    public class StatementdetailsData:BaseData
    {
        private string _CustomerID;
        private string _AccountNumber;
        private string _fromDate;
        private string _toDate;

        public string CustomerID { get => _CustomerID; set => _CustomerID = value; }
        public string AccountNumber { get => _AccountNumber; set => _AccountNumber = value; }
        public string fromDate { get => _fromDate; set => _fromDate = value; }
        public string toDate { get => _toDate; set => _toDate = value; }
    }
}
