using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class CountryMaster
    {
        public CountryMaster()
        {
            Customers = new HashSet<Customer>();
            StateMasters = new HashSet<StateMaster>();
        }

        public short CountryId { get; set; }
        public string CountryName { get; set; } = null!;

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<StateMaster> StateMasters { get; set; }
    }
}
