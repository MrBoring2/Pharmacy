using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PharmacyApp.Models.POCOModels
{
    public class Analizer
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("analizerName")]
        public string AnalizerName { get; set; }
        [JsonPropertyName("laboratoryServiceToAnalizers")]
        public ICollection<LaboratoryServiceToAnalizer> LaboratoryServiceToAnalizer { get; set; }
    }
}
