namespace customerSdmodule.ModelClass.ChequeEmployeeVerify
{
    public class ChequeEmployeeVerifyRequest : Request
    {
        public ChequeEmployeeVerifyRequest()
        {
            ChequeEmployeeVerifyRequest._Requesttype = "ChequeEmployeeVerifyRequest";
        }

        public ChequeEmployeeVerifyApi Data { get => (ChequeEmployeeVerifyApi)base.Data; set => base.Data = (ChequeEmployeeVerifyApi)value; }
    }
}