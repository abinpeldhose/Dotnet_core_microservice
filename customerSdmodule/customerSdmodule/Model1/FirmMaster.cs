using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class FirmMaster
    {
        public FirmMaster()
        {
            BranchMasters = new HashSet<BranchMaster>();
            Customers = new HashSet<Customer>();
            EmployeeDepartmentMasters = new HashSet<EmployeeDepartmentMaster>();
            EmployeeMasters = new HashSet<EmployeeMaster>();
            GeneralParameters = new HashSet<GeneralParameter>();
            RegionMasters = new HashSet<RegionMaster>();
            SdMasters = new HashSet<SdMaster>();
            SdSchemes = new HashSet<SdScheme>();
            SdVerifications = new HashSet<SdVerification>();
        }

        public byte FirmId { get; set; }
        public string? FirmName { get; set; }
        public string? FirmAbbr { get; set; }
        public string? FirmAddress { get; set; }
        public int? StatusId { get; set; }

        public virtual StatusMaster? Status { get; set; }
        public virtual ICollection<BranchMaster> BranchMasters { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<EmployeeDepartmentMaster> EmployeeDepartmentMasters { get; set; }
        public virtual ICollection<EmployeeMaster> EmployeeMasters { get; set; }
        public virtual ICollection<GeneralParameter> GeneralParameters { get; set; }
        public virtual ICollection<RegionMaster> RegionMasters { get; set; }
        public virtual ICollection<SdMaster> SdMasters { get; set; }
        public virtual ICollection<SdScheme> SdSchemes { get; set; }
        public virtual ICollection<SdVerification> SdVerifications { get; set; }
    }
}
