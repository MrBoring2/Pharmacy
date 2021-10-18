using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PharmacyApp.Models.POCOModels
{
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("fullName")]
        public string FullName { get; set; }
        [JsonPropertyName("login")]
        public string Login { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("ip")]
        public string Ip { get; set; }
        [JsonPropertyName("lasEnterDate")]
        public DateTime LastEnterDate { get; set; }
        [JsonPropertyName("ServicesCodes")]
        public string ServicesCodes { get; set; }
        [JsonPropertyName("roleId")]
        public int RoleId { get; set; }
        [JsonPropertyName("role")]
        public Role Role { get; set; }
    }
}
