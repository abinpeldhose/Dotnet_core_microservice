namespace customerSdmodule.ModelClass.VerifyOtp
{
    public class VerifyOtpData:BaseData
    {
        private int _transactionid;
        private int _otp;

        public int Transactionid { get => _transactionid; set => _transactionid = value; }
        public int Otp { get => _otp; set => _otp = value; }
    }
}
