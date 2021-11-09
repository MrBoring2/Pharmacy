using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PharmacyApp.Models.POCOModels
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
