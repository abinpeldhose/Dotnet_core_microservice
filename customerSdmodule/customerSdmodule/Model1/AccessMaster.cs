using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class AccessMaster
    {
        public AccessMaster()
        {
            EmployeeMasters = new HashSet<EmployeeMaster>();
        }

        public byte AccessId { get; set; }
        public string? AccessName { get; set; }

        public virtual ICollection<EmployeeMaster> EmployeeMasters { get; set; }
    }
}
