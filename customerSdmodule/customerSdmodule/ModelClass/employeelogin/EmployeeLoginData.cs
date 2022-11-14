namespace customerSdmodule.ModelClass.employeelogin
{
    public class EmployeeLoginData:BaseData
    {
        public int ModuleId { get; set; }
        public byte FirmId { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
