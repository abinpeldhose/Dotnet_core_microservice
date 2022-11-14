using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class MaritalStatusMaster
    {
        public MaritalStatusMaster()
        {
            Customers = new HashSet<Customer>();
        }

        public string MaritalStatusId { get; set; } = null!;
        public string? MaritalStatus { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
