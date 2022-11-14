namespace customerSdmodule.ModelClass.AddNotes
{
    public class AddNoteRequest:Request
    {
        public AddNoteRequest()
        {
            AddNoteRequest._Requesttype = "AddNoteRequest";
        }

        public AddNoteAPI Data { get => (AddNoteAPI)base.Data; set => base.Data = (AddNoteAPI)value; }
    }
}
