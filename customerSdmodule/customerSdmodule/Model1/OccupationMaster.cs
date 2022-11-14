using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class OccupationMaster
    {
        public int OccupationId { get; set; }
        public string OccupationName { get; set; } = null!;
        public int StatusId { get; set; }

        public virtual StatusMaster Status { get; set; } = null!;
    }
}
