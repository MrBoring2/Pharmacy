using System;
using System.Collections.Generic;

#nullable disable

namespace PharmacyApi.Data.DBData.EntityModels
{
    public partial class InsuranceСompany
    {
        public InsuranceСompany()
        {
            InvoicesIssueds = new HashSet<InvoicesIssued>();
            Patients = new HashSet<Patient>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Inn { get; set; }
        public string CheckingAccount { get; set; }
        public string Bic { get; set; }
        public string Country { get; set; }

        public virtual ICollection<InvoicesIssued> InvoicesIssueds { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
    }
}
