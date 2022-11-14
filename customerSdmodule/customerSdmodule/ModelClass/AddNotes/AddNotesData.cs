namespace customerSdmodule.ModelClass.AddNotes
{
    public class AddNotesData:BaseData
    {
        public int FirmId { get; set; }
        public int BranchId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime NoteDate { get; set; }
        public string Description { get; set; }
    }
}
