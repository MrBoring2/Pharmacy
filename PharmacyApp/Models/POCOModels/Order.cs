using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PharmacyApp.Models.POCOModels
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
        [JsonPropertyName("laboratoryServicesToOrders")]
        public ICollection<LaboratoryServicesToOrder> LaboratoryServicesToOrders { get; set; }
    }
}
