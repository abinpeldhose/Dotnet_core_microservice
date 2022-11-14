using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class UserTable
    {
        public byte? FirmId { get; set; }
        public short BranchId { get; set; }
        public string? EmpName { get; set; }
        public byte? DepartmentId { get; set; }
        public string? DepName { get; set; }
    }
}
