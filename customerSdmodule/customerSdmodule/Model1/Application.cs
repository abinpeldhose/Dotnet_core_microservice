using System.ComponentModel.DataAnnotations;

namespace customerSdmodule.Model1
{
   
        public partial class Application
        {
        [Key]
            public int? AppNo { get; set; }
            public byte? FirmId { get; set; }
            public string? VersionNo { get; set; }
            public DateTime? BuildDate { get; set; }
            public byte? ModuleId { get; set; }
            public string? Builder { get; set; }
            public string? UserType { get; set; }
    }
    
}
