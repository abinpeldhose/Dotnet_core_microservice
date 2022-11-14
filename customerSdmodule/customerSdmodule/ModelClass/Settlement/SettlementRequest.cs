

namespace customerSdmodule.ModelClass.Settlement
{
    public class SettlementRequest : Request
    {
        public SettlementRequest()
        {
            SettlementRequest._Requesttype = "SettlementRequest";
        }

        public SettlementAPI Data { get => (SettlementAPI)base.Data; set => base.Data = (SettlementAPI)value; }

    }
}
