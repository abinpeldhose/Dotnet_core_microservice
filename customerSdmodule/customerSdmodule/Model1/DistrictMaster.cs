using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class DistrictMaster
    {
        public DistrictMaster()
        {
            BranchMasters = new HashSet<BranchMaster>();
            PostMasters = new HashSet<PostMaster>();
        }

        public short DistrictId { get; set; }
        public string DistrictName { get; set; } = null!;
        public byte StateId { get; set; }

        public virtual StateMaster State { get; set; } = null!;
        public virtual ICollection<BranchMaster> BranchMasters { get; set; }
        public virtual ICollection<PostMaster> PostMasters { get; set; }
    }
}
