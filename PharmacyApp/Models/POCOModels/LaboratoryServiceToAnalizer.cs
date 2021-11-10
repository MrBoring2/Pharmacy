using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PharmacyApp.Models.POCOModels
{
    public class LaboratoryServiceToAnalizer
    {
        [JsonPropertyName("analizerId")]
        public int AnalizerId { get; set; }
        [JsonPropertyName("laboratoryServiceId")]
        public int LaboratoryServiceId { get; set; }
        [JsonPropertyName("analizer")]
        public virtual Analizer Analizer { get; set; }
        [JsonPropertyName("laboratoryService")]
        public virtual LaboratoryService LaboratoryService { get; set; }
    }
}
