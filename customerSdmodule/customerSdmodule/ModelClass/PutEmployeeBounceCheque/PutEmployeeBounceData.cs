using System.Text.Json;
namespace customerSdmodule.ModelClass.PutEmployeeBounceCheque
{
    public class PutEmployeeBounceData : BaseData
    {

        private string chequeno;
        private string depositid;
        private int employeecode;
        private string rejectreason;
        private string cleardate;

        public string Cheque_no { get => chequeno; set => chequeno = value; }
        public string DepositId { get => depositid; set => depositid = value; }
        public int EmpId { get => employeecode; set => employeecode = value; }
        public string RejectReason { get => rejectreason; set => rejectreason = value; }
        public string Cleardt { get => cleardate; set => cleardate = value; }
    }
}
