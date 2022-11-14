using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class EmployeeDesiginationMaster
    {
        public EmployeeDesiginationMaster()
        {
            EmployeeMasters = new HashSet<EmployeeMaster>();
        }

        public byte DesignationId { get; set; }
        public string Designation { get; set; } = null!;
        public byte GradeId { get; set; }
        public byte? ProfileGrade { get; set; }

        public virtual ICollection<EmployeeMaster> EmployeeMasters { get; set; }
    }
}
