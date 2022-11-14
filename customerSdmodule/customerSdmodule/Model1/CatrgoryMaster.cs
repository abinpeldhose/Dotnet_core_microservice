using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class CatrgoryMaster
    {
        public CatrgoryMaster()
        {
            SdMasters = new HashSet<SdMaster>();
        }

        public bool CatrgoryId { get; set; }
        public string? Catrgory { get; set; }

        public virtual ICollection<SdMaster> SdMasters { get; set; }
    }
}
