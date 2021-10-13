using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using PharmacyApi.Data.DBData;
using PharmacyApi.Data.DBData.EntityModels;
using PharmacyApi.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PharmacyApi.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private PharmacyContext _context;

        public AuthenticationController(PharmacyContext context)
        {
            _context = context;
        }

        // POST api/<AuthenticationController>
        [HttpPost("/token")]
        public async Task<IActionResult> Token(string login, string password)
        {
            var identity = await GetIdentity(login, password);
            if(identity == null)
            {
                return BadRequest("Неверное имя пользователя или пароль");
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Ok(response);
        }

        private async Task<ClaimsIdentity> GetIdentity(string login, string password)
        {
            User user = await _context.Users.Include(r => r.Role)
                .FirstOrDefaultAsync(x => x.Login == login && x.Password == password);

            if(user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.RoleName)
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}
