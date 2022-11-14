using MACOM.Contracts;

namespace customerSdmodule
{
    public class Detail
    {
        public Data Data { get; set; }
    }
    public class Data
    {
        public SubledgerRequest SubLedger { get; set; }
    }
    //public class SubLedger
    //{
    //    public SubledgerRequest SubLedger { get; set; }
    //}
}
