namespace customerSdmodule.ModelClass.BHApprove
{
    public class BHApproveRequest : Request
    {
        public BHApproveRequest()
        {
            BHApproveRequest._Requesttype = "BhApproveRequest";
        }

        public BHApproveAPI Data { get => (BHApproveAPI)base.Data; set => base.Data = (BHApproveAPI)value; }

    }
}
