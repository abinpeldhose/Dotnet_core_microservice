using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class Alert
    {
        public int AlertId { get; set; }
        public string? Type { get; set; }
        public string? Subject { get; set; }
        public DateTime? AlertDate { get; set; }
        public string? AlertDescription { get; set; }
        public string? Id { get; set; }
        public DateTime? ReadDate { get; set; }
    }
}
