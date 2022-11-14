namespace customerSdmodule.ModelClass.employeelogin
{
    public class EmployeeLoginRequest:Request
    {
        public EmployeeLoginRequest()
        {
            EmployeeLoginRequest._Requesttype = "EmployeeLoginRequest";
        }

        public EmployeeLoginApi Data { get => (EmployeeLoginApi)base.Data; set => base.Data = (EmployeeLoginApi)value; }
    }
}
