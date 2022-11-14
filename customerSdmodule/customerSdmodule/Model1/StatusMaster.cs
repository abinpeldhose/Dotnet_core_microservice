using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class StatusMaster
    {
        public StatusMaster()
        {
            BranchMasters = new HashSet<BranchMaster>();
            DepositTypeMasters = new HashSet<DepositTypeMaster>();
            EmployeeDepartmentMasters = new HashSet<EmployeeDepartmentMaster>();
            EmployeeMasters = new HashSet<EmployeeMaster>();
            FirmMasters = new HashSet<FirmMaster>();
            OccupationMasters = new HashSet<OccupationMaster>();
            SdMasterChqstatusNavigations = new HashSet<SdMaster>();
            SdMasterIncentives = new HashSet<SdMaster>();
            SdMasterMinorNavigations = new HashSet<SdMaster>();
            SdMasterNomineeNavigations = new HashSet<SdMaster>();
            SdMasterStatuses = new HashSet<SdMaster>();
            SdSchemes = new HashSet<SdScheme>();
        }

        public int StatusId { get; set; }
        public string? Status { get; set; }
        public string? AddedBy { get; set; }

        public virtual ICollection<BranchMaster> BranchMasters { get; set; }
        public virtual ICollection<DepositTypeMaster> DepositTypeMasters { get; set; }
        public virtual ICollection<EmployeeDepartmentMaster> EmployeeDepartmentMasters { get; set; }
        public virtual ICollection<EmployeeMaster> EmployeeMasters { get; set; }
        public virtual ICollection<FirmMaster> FirmMasters { get; set; }
        public virtual ICollection<OccupationMaster> OccupationMasters { get; set; }
        public virtual ICollection<SdMaster> SdMasterChqstatusNavigations { get; set; }
        public virtual ICollection<SdMaster> SdMasterIncentives { get; set; }
        public virtual ICollection<SdMaster> SdMasterMinorNavigations { get; set; }
        public virtual ICollection<SdMaster> SdMasterNomineeNavigations { get; set; }
        public virtual ICollection<SdMaster> SdMasterStatuses { get; set; }
        public virtual ICollection<SdScheme> SdSchemes { get; set; }
    }
}
