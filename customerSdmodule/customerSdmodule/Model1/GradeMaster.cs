using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class GradeMaster
    {
        public byte GradeId { get; set; }
        public string Grade { get; set; } = null!;
        public string GradeAbbr { get; set; } = null!;
    }
}
