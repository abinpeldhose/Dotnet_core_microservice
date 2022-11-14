namespace customerSdmodule.ModelClass.GetNotes
{
    public class GetNotesRequest:Request
    {
        public GetNotesRequest()
        {
            GetNotesRequest._Requesttype = "GetNotesRequest";
        }

        public GetNotesAPI Data { get => (GetNotesAPI)base.Data; set => base.Data = (GetNotesAPI)value; }
    }
}
