using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PharmacyMobile.Models.POCO_Models
{
    public class Order
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("patientId")]
        public int PatientId { get; set; }
        [JsonPropertyName("dateOfCreation")]
        public string DateOfCreation { get; set; }
        [JsonPropertyName("barcode")]
        public string Barcode { get; set; }
        [JsonPropertyName("patient")]
        public Patient Patient { get; set; }
    }
}
