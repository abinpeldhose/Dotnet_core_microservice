namespace customerSdmodule
{
    public class Request
    {
        private string _JwtToken;
        private string _type;
        private decimal _ver;
        private string _hash;
        protected static string _Requesttype = "Request";
        private object data;

        public string Type { get => _type; set => _type = value; }
        public decimal Ver { get => _ver; set => _ver = value; }
        public static string Requesttype { get => _Requesttype; }
        public string JwtToken { get => _JwtToken; set => _JwtToken = value; }
        public  object Data { get => data; set => data = value; }
        public string Hash { get => _hash; set => _hash = value; }
    }
}
