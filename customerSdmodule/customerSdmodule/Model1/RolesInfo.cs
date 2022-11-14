using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class RolesInfo
    {
        public byte RoleId { get; set; }
        public string? RoleName { get; set; }
        public byte? FirmId { get; set; }
        public short BranchId { get; set; }
    }
}
