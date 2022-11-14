namespace customerSdmodule.ModelClass.Validateuserid
{
    public class ValidateUserIdRequest:Request
    {
        public ValidateUserIdRequest()
        {
            ValidateUserIdRequest._Requesttype = "ValidateUserIdRequest";
        }

        public ValidateUserIdApi Data { get => (ValidateUserIdApi)base.Data; set => base.Data = (ValidateUserIdApi)value; }
    }
}
