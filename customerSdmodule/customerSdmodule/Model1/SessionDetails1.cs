using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class SessionDetails1
    {

        public string SessionId { get; set; } = null!;
        public DateTime? LoginTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string? UserId { get; set; }
        public byte? MaxTime { get; set; }
    }
}
