using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PharmacyApp.Models.POCOModels
{
    public class AuthenticationLogger : BaseModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("loginDate")]
        public DateTime LoginDate { get; set; }
        [JsonPropertyName("attempt")]
        public bool Attempt { get; set; }
        [JsonPropertyName("user")]
        public User User { get; set; }
    }
}
