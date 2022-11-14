namespace customerSdmodule.ModelClass.DeleteNotes
{
    public class DeleteNoteRequest:Request
    {
        public DeleteNoteRequest()
        {
            DeleteNoteRequest._Requesttype = "DeleteNoteRequest";
        }

        public DeleteNoteAPI Data { get => (DeleteNoteAPI)base.Data; set => base.Data = (DeleteNoteAPI)value; }
    }
}
