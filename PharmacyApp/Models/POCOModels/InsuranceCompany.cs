using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PharmacyApp.Models.POCOModels
{
    public class InsuranceCompany : BaseModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("inn")]
        public string Inn { get; set; }
        [JsonPropertyName("checkingAccount")]
        public string CheckingAccount { get; set; }
        [JsonPropertyName("bic")]
        public string Bic { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; }
    }
}
