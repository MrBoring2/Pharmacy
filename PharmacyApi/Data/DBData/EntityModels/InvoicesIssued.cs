using System;
using System.Collections.Generic;

#nullable disable

namespace PharmacyApi.Data.DBData.EntityModels
{
    public partial class InvoicesIssued
    {
        public int InsuranceCompanyId { get; set; }
        public int UserId { get; set; }
        public DateTime StartPeriod { get; set; }
        public DateTime EndPeriod { get; set; }
        public decimal Price { get; set; }

        public virtual InsuranceСompany InsuranceCompany { get; set; }
    }
}
