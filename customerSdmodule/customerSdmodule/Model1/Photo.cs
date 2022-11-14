using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace customerSdmodule.Model1
{
    public partial class Photo
    {
        [Key]
        public string? Custid { get; set; }
        public string? Image { get; set; }
    }

}
