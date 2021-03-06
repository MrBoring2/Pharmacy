using System;
using System.Collections.Generic;

#nullable disable

namespace PharmacyApi.Data.DBData.EntityModels
{
    public partial class Patient
    {
        public Patient()
        {
            Orders = new HashSet<Order>();
        }

        public int PatientId { get; set; }
        public Guid Guid { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string SosialSecNumber { get; set; }
        public string Ein { get; set; }
        public string SosialType { get; set; }
        public string Telephone { get; set; }
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public byte[] DateOfBirth { get; set; }
        public string Ua { get; set; }
        public int? InsuranceCompanyId { get; set; }
        public string IpAddress { get; set; }

        public virtual InsuranceСompany InsuranceCompany { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
