namespace customerSdmodule.ModelClass.EligibleSchemes
{
    public class GetEligibleRequest:Request
    {

        public GetEligibleRequest()
        {
            GetEligibleRequest._Requesttype = "GetEligibleRequest";
        }

        public GetEligibleApi Data
        {
            get => (GetEligibleApi)base.Data; set => base.Data = (GetEligibleApi)value;

        }

    }
}
