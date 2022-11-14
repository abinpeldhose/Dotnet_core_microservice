using System.ComponentModel.DataAnnotations;

namespace customerSdmodule.Model1
{
    public partial class SdNote
    {
        public int? FirmId { get; set; }
        public int? BrachId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? NoteDate { get; set; }
        public string? NoteDescription { get; set; }
        public int NoteId { get; set; }
    }
}
