using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Models.POCOModels
{
    public class TokenModel : BaseModel
    {
        public string access_token { get; set; }
        public string user_name_surname { get; set; }
        public string role_name { get; set; }
        public string role_id { get; set; }
        public string user_login { get; set; }
    }
}
