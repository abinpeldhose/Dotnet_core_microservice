using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class LocalbodyMaster
    {
        public LocalbodyMaster()
        {
            BranchMasters = new HashSet<BranchMaster>();
        }

        public byte LocalbodyId { get; set; }
        public string? LocalbodyName { get; set; }

        public virtual ICollection<BranchMaster> BranchMasters { get; set; }
    }
}
