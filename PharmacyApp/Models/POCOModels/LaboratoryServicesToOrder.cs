using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PharmacyApp.Models.POCOModels
{
    public class LaboratoryServicesToOrder
    {
        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }
        [JsonPropertyName("laboratoryServiceId")]
        public int LaboratoryServiceId { get; set; }
        [JsonPropertyName("result")]
        public string Result { get; set; }
        [JsonPropertyName("dateOfFinished")]
        public string DateOfFinished { get; set; }
        [JsonPropertyName("accepted")]
        public bool? Accepted { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("analyzerId")]
        public int? AnalyzerId { get; set; }
        [JsonPropertyName("userId")]
        public int? UserId { get; set; }
        [JsonPropertyName("analizer")]
        public Analizer Analyzer { get; set; }
        [JsonPropertyName("laboratoryService")]
        public LaboratoryService LaboratoryService { get; set; }
        [JsonPropertyName("order")]
        public Order Order { get; set; }
        [JsonPropertyName("user")]
        public User User { get; set; }
    }
}
