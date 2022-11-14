

namespace customerSdmodule.ModelClass.StatementTransactionDetails



{
    public class StatementTransactionDetailsRequest: Request
    {

        public StatementTransactionDetailsRequest()
        {
            StatementTransactionDetailsRequest._Requesttype = "StatementTransactionDetailsRequest";
        }

        public StatementTransatctionDetailsApi Data { get => (StatementTransatctionDetailsApi)base.Data; set => base.Data = (StatementTransatctionDetailsApi)value; }
    }
}
