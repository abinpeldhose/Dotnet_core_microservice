namespace customerSdmodule.ModelClass.DeleteBH
{
    public class DeleteBHRequest:Request
    {
        public DeleteBHRequest()
        {
            DeleteBHRequest._Requesttype = "DeleteBHRequest";
        }

        public DeleteBHAPI Data { get => (DeleteBHAPI)base.Data; set => base.Data = (DeleteBHAPI)value; }
    }
}
