using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace customerSdmodule.Model1
{
    public partial class SessionDetail
    {
        [Key]
        public string? SessionId { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LastUpdatetime { get; set; }
        public int? UserId { get; set; }
    }
}
