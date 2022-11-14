using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class RegionMaster
    {
        public RegionMaster()
        {
            BranchMasters = new HashSet<BranchMaster>();
        }

        public byte? FirmId { get; set; }
        public byte RegionId { get; set; }
        public string RegionName { get; set; } = null!;

        public virtual FirmMaster? Firm { get; set; }
        public virtual ICollection<BranchMaster> BranchMasters { get; set; }
    }
}
