using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class Nomine
    {
        public string? Name { get; set; }
        public DateTime? Dob { get; set; }
        public string? Relation { get; set; }
        public string? Fathername { get; set; }
        public string? Housename { get; set; }
        public string? Address { get; set; }
        public string? Location { get; set; }
        public string VerifyId { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? CoApplicantName { get; set; }
        public DateTime? CoApplicantDob { get; set; }
        public string? CoApplicantHousename { get; set; }
        public string? CoApplicantAddress { get; set; }
        public string? CoApplicantPhno { get; set; }
        public string? SalesCode { get; set; }
    }
}
