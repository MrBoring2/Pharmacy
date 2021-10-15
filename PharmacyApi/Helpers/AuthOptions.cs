using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApi.Helpers
{
    public class AuthOptions
    {
        public const string ISSUER = "PharmacyAuthServer";
        public const string AUDIENCE = "PharmacyAuthClient";
        private const string KEY = "pharmacysecretkey";
        public const int LIFETIME = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
