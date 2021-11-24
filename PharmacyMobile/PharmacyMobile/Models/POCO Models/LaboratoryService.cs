using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PharmacyMobile.Models.POCO_classes
{
    public class LaboratoryService
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}
