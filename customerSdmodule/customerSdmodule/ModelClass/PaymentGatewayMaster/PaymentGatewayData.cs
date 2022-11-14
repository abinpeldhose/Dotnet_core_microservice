namespace customerSdmodule.ModelClass.PaymentGatewayMaster
{
    public class PaymentGatewayData:BaseData
    {
        private string _usertype;
        private string _paymenttype;

        public string Usertype { get => _usertype; set => _usertype = value; }
        public string Paymenttype { get => _paymenttype; set => _paymenttype = value; }
    }
}
