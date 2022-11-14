using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class CitizenMaster
    {
        public CitizenMaster()
        {
            SdMasters = new HashSet<SdMaster>();
        }

        public string CitizenId { get; set; } = null!;
        public string CitizenType { get; set; } = null!;

        public virtual ICollection<SdMaster> SdMasters { get; set; }
    }
}
