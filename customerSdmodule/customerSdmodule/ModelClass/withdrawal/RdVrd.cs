namespace customerSdmodule.ModelClass.withdrawal
{
    public class RdVrd
    {
        public int transno { get; set; }
        public string rcptarr { get; set; }
        public string errMessage { get; set; }
        public int errStat { get; set; }

        public RDvrd_sub status { get; set; }

    }
    public class RDvrd_sub
    {
        public dynamic flag { get; set; }
        public dynamic code { get; set; }
        public dynamic message { get; set; }
        public dynamic timeStamp { get; set; }
    }
}
