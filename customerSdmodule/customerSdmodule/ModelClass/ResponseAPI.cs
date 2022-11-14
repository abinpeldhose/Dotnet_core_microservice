namespace customerSdmodule.ModelClass
{
    public class ResponseData
    {
        public string jwtToken { get; set; }

        public System.Text.Json.JsonElement data { get;set; }

        public string hash { get; set; }

        public int responseCode { get; set; }

        public string deviceToken { get; set; }
        
    }
}
