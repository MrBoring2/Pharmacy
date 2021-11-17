using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PharmacyApp.Models.POCOModels
{
    public class InvoiceIssue
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("insuranceCompanyId")]
        public int InsuranceCompanyId { get; set; }
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
        [JsonPropertyName("startPeriod")]
        public double StartPeriod { get; set; }
        [JsonPropertyName("endPeriod")]
        public double EndPeriod { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("insuranceCompany")]

        public InsuranceСompany InsuranceCompany { get; set; }
    }
}
