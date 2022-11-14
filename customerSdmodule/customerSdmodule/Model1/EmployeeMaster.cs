using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class EmployeeMaster
    {
        public short BranchId { get; set; }
        public byte? FirmId { get; set; }
        public int EmpCode { get; set; }
        public string? EmpName { get; set; }
        public int? StatusId { get; set; }
        public DateTime? JoinDt { get; set; }
        public byte? AccessId { get; set; }
        public byte? DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public int PostId { get; set; }
        public byte? GradeId { get; set; }
        public int? EmpType { get; set; }
        public string? Phone { get; set; }
        public Guid? Password { get; set; }

        public virtual AccessMaster? Access { get; set; }
        public virtual BranchMaster Branch { get; set; } = null!;
        public virtual EmployeeDepartmentMaster? Department { get; set; }
        public virtual EmployeeDesiginationMaster? Designation { get; set; }
        public virtual FirmMaster? Firm { get; set; }
        public virtual EmployeePostMaster? Post { get; set; }
        public virtual StatusMaster? Status { get; set; }
    }
}
