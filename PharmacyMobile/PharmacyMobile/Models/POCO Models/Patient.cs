using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PharmacyMobile.Models.POCO_Models
{
    public class Patient
    {
        [JsonPropertyName("patientId")]
        public int PatientId { get; set; }
        [JsonPropertyName("guid")]
        public string Guid { get; set; }
        [JsonPropertyName("fullName")]
        public string FullName { get; set; }
        [JsonPropertyName("login")]
        public string Login { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("sosialSecNumber")]
        public string SosialSecNumber { get; set; }
        [JsonPropertyName("ein")]
        public string Ein { get; set; }
        [JsonPropertyName("sosialType")]
        public string SosialType { get; set; }
        [JsonPropertyName("telephone")]
        public string Telephone { get; set; }
        [JsonPropertyName("passportSeries")]
        public string PassportSeries { get; set; }
        [JsonPropertyName("passportNumber")]
        public string PassportNumber { get; set; }
        [JsonPropertyName("dateOfBirth")]
        public double DateOfBirth { get; set; }
        [JsonPropertyName("ua")]
        public string Ua { get; set; }
        [JsonPropertyName("insuranceCompanyId")]
        public int? InsuranceCompanyId { get; set; }
        [JsonPropertyName("ipAddress")]
        public string IpAddress { get; set; }
        [JsonPropertyName("insuranceCompany")]
        public InsuranceCompany InsuranceCompany { get; set; }
        [JsonPropertyName("orders")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
