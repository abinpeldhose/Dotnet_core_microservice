using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class Customer
    {
        public Customer()
        {
            SdMasters = new HashSet<SdMaster>();
            SdVerifications = new HashSet<SdVerification>();
        }

        public byte? FirmId { get; set; }
        public short? BranchId { get; set; }
        public string CustId { get; set; } = null!;
        public string? CustName { get; set; }
        public string? MaritalStatus { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public decimal? SpousePrefixId { get; set; }
        public string? SpouseName { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public int? PinNo { get; set; }
        public string? HouseName { get; set; }
        public string? Locality { get; set; }
        public string? Street { get; set; }
        public string? LandMark { get; set; }
        public short? CountryId { get; set; }
        public int? AltPinNo { get; set; }
        public string? AltHouseName { get; set; }
        public string? AltLocality { get; set; }
        public DateTime? LastModiDate { get; set; }
        public string? ShareNo { get; set; }
        public byte? Sharecount { get; set; }
        public string? PsdNo { get; set; }
        public string? CardNo { get; set; }
        public int? SancationBy { get; set; }

        public virtual BranchMaster? Branch { get; set; }
        public virtual CountryMaster? Country { get; set; }
        public virtual FirmMaster? Firm { get; set; }
        public virtual MaritalStatusMaster? MaritalStatusNavigation { get; set; }
        public virtual ICollection<SdMaster> SdMasters { get; set; }
        public virtual ICollection<SdVerification> SdVerifications { get; set; }
    }
}
