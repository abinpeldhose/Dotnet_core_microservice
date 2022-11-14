using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class RoleUser
    {
        public int EmpCode { get; set; }
        public byte? RoleId { get; set; }
        public byte? FirmId { get; set; }
        public short BranchId { get; set; }
    }
}
