using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class EmployeePostMaster
    {
        public EmployeePostMaster()
        {
            EmployeeMasters = new HashSet<EmployeeMaster>();
        }

        public int PostId { get; set; }
        public string PostName { get; set; } = null!;

        public virtual ICollection<EmployeeMaster> EmployeeMasters { get; set; }
    }
}
