using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class EmployeeDepartmentMaster
    {
        public EmployeeDepartmentMaster()
        {
            EmployeeMasters = new HashSet<EmployeeMaster>();
        }

        public byte? FirmId { get; set; }
        public int DepId { get; set; }
        public string? DepName { get; set; }
        public int? Status { get; set; }
        public int? DepHead { get; set; }

        public virtual FirmMaster? Firm { get; set; }
        public virtual StatusMaster? StatusNavigation { get; set; }
        public virtual ICollection<EmployeeMaster> EmployeeMasters { get; set; }
    }
}
