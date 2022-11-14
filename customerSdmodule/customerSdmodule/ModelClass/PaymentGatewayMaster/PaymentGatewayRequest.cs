namespace customerSdmodule.ModelClass.PaymentGatewayMaster
{
    public class PaymentGatewayRequest : Request
    {

        public PaymentGatewayRequest()
        {
            PaymentGatewayRequest._Requesttype = "PaymentGatewayRequest";
        }

        public PaymentGatewayApi Data { get => (PaymentGatewayApi)base.Data; set => base.Data = (PaymentGatewayApi)value; }
    }
}
