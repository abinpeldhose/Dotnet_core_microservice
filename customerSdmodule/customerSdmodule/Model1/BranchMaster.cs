using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class BranchMaster
    {
        public BranchMaster()
        {
            Customers = new HashSet<Customer>();
            EmployeeMasters = new HashSet<EmployeeMaster>();
            SdMasters = new HashSet<SdMaster>();
            SdSchemes = new HashSet<SdScheme>();
            SdVerifications = new HashSet<SdVerification>();
        }

        public byte FirmId { get; set; }
        public short BranchId { get; set; }
        public string BranchName { get; set; } = null!;
        public string? BranchAbbr { get; set; }
        public string? BranchAddr { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public int? Pincode { get; set; }
        public byte? RegionId { get; set; }
        public short? DistrictId { get; set; }
        public byte? StateId { get; set; }
        public DateTime? TraDt { get; set; }
        public string? UptoDate { get; set; }
        public int? StatusId { get; set; }
        public DateTime? InaugurationDt { get; set; }
        public byte? LocalBody { get; set; }
        public string? BranchAdd1 { get; set; }
        public string? BranchAdd2 { get; set; }
        public string? BranchAdd3 { get; set; }
        public string? BranchAdd4 { get; set; }
        public string? BranchAdd5 { get; set; }

        public virtual DistrictMaster? District { get; set; }
        public virtual FirmMaster Firm { get; set; } = null!;
        public virtual LocalbodyMaster? LocalBodyNavigation { get; set; }
        public virtual RegionMaster? Region { get; set; }
        public virtual StateMaster? State { get; set; }
        public virtual StatusMaster? Status { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<EmployeeMaster> EmployeeMasters { get; set; }
        public virtual ICollection<SdMaster> SdMasters { get; set; }
        public virtual ICollection<SdScheme> SdSchemes { get; set; }
        public virtual ICollection<SdVerification> SdVerifications { get; set; }
    }
}
