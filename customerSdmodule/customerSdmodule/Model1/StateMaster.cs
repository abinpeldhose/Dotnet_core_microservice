using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class StateMaster
    {
        public StateMaster()
        {
            BranchMasters = new HashSet<BranchMaster>();
            DistrictMasters = new HashSet<DistrictMaster>();
        }

        public byte StateId { get; set; }
        public string StateName { get; set; } = null!;
        public short CountryId { get; set; }
        public string? StateAbbr { get; set; }

        public virtual CountryMaster Country { get; set; } = null!;
        public virtual ICollection<BranchMaster> BranchMasters { get; set; }
        public virtual ICollection<DistrictMaster> DistrictMasters { get; set; }
    }
}
