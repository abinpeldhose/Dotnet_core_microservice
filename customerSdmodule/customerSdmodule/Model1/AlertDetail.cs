using System.ComponentModel.DataAnnotations;

namespace customerSdmodule.Model1
{
    public partial class AlertDetail
    {
        [Key]
        public byte? Slno { get; set; }
        public string? Alerttype { get; set; }
        public string? Image { get; set; }

    }
}
