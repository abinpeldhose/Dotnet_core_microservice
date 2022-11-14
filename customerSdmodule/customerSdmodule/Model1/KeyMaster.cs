namespace customerSdmodule.Model1
{
    public partial class KeyMaster
    {
        public byte FirmId { get; set; }
        public short BranchId { get; set; }
        public byte ModuleId { get; set; }
        public int KeyId { get; set; }
        public string? Description { get; set; }
        public int? Value { get; set; }
    }
}
