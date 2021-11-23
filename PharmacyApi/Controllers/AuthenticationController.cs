using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PharmacyApi.Data.DBData;
using PharmacyApi.Data.DBData.EntityModels;
using PharmacyApi.Helpers;
using PharmacyApi.Services.Hubs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PharmacyApi.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private IHubContext<PharmacyHub> _hubContext;

        private PharmacyContext _context;

        public AuthenticationController(PharmacyContext context, IHubContext<PharmacyHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        [HttpPost("/authPatient")]
        public async Task<IActionResult> AuthPatient(string login, string password)
        {
            Patient patient = await _context.Patients.Include(r => r.InsuranceCompany)
               .FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            return Ok();
        }


        // POST api/<AuthenticationController>
        [HttpPost("/token")]
        public async Task<IActionResult> Token(string login, string password)
        {
            var identity = await GetIdentity(login, password);

            if (identity == null)
            {
                return BadRequest("Неверное имя пользователя или пароль");
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromHours(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                user_id = identity.FindFirst("user_id").Value.ToString(),
                user_name_surname = identity.FindFirst("user_name_surname").Value.ToString(),
                user_login = identity.FindFirst("user_login").Value.ToString(),
                role_id = identity.FindFirst("role_id").Value.ToString(),
                role_name = identity.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value.ToString()
            };

            return Ok(response);
        }

        private void LogUser(string login, bool atempt)
        {
            var logger = new AuthenticationLogger();

            logger.LoginDate = DateTime.Now;
            logger.User = _context.Users.Where(p => p.Login == login).FirstOrDefault();
            logger.Attempt = atempt;

            _context.AuthenticationLoggers.Add(logger);
            _context.SaveChanges();
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            _hubContext.Clients.All.SendAsync("UpdateLogs", JsonSerializer.Serialize(_context.AuthenticationLoggers.Include(p => p.User).ToList(), options));
        }

        private async Task<ClaimsIdentity> GetIdentity(string login, string password)
        {
            User user = await _context.Users.Include(r => r.Role)
                .FirstOrDefaultAsync(x => x.Login == login && x.Password == password);

            bool atempt;
            if (user is null)
                atempt = false;
            else atempt = true;

            LogUser(login, atempt);


            if (user != null)
            {
                user.LastEnterDate = DateTime.Now;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();

                var claims = new List<Claim>
                {
                    new Claim("user_login", user.Login),
                    new Claim("user_id", user.Id.ToString()),
                    new Claim("user_name_surname", user.FullName),
                    new Claim("role_id", user.Role.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.RoleName.ToString())
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                //await AddUserToGroup((Roles)Convert.ToInt32(claimsIdentity.FindFirst("role_id").Value), claimsIdentity);

                return claimsIdentity;
            }

            return null;
        }

        private async Task AddUserToGroup(Roles userRole, ClaimsIdentity identity)
        {
            string group = string.Empty;
            switch (userRole)
            {
                case Roles.Administrator:
                    group = HubGroups.admins;
                    break;
                case Roles.Laboratorian:
                    group = HubGroups.laboratorians;
                    break;
                case Roles.LaboratorianResearcher:
                    group = HubGroups.laboratorians;
                    break;
                case Roles.Accountant:
                    group = HubGroups.accountents;
                    break;
                case Roles.Patient:
                    group = HubGroups.patients;
                    break;
                default:
                    break;
            }

            // var d = HttpContext.Request.Cookies["connectionId"];
            var d = identity.FindFirst("user_login").Value;

            await _hubContext.Groups.AddToGroupAsync(identity.FindFirst("user_login").Value, group);
        }
    }
}
