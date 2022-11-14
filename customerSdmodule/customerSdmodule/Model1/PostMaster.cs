namespace customerSdmodule.Model1
{
    public partial class PostMaster
    {
        public int SrNumber { get; set; }
        public int PinCode { get; set; }
        public string? PostOffice { get; set; }
        public short? DistrictId { get; set; }

        public virtual DistrictMaster? District { get; set; }
    }
}
