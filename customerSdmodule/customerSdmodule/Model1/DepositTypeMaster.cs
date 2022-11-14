using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class DepositTypeMaster
    {
        public DepositTypeMaster()
        {
            SdMasters = new HashSet<SdMaster>();
            SdVerifications = new HashSet<SdVerification>();
        }

        public string TypeId { get; set; } = null!;
        public string? TypeName { get; set; }
        public int? StatusId { get; set; }

        public virtual StatusMaster? Status { get; set; }
        public virtual ICollection<SdMaster> SdMasters { get; set; }
        public virtual ICollection<SdVerification> SdVerifications { get; set; }
    }
}
