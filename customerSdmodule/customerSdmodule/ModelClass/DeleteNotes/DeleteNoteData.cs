namespace customerSdmodule.ModelClass.DeleteNotes
{
    public class DeleteNoteData:BaseData
    {
        public int EmployeeId { get; set; }
        public DateTime NoteDate { get; set; }
        public string NoteDescription { get; set; }
        public int NoteId { get; set; }
    }
}
