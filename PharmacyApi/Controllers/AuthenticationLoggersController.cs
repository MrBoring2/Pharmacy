using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyApi.Data.DBData;
using PharmacyApi.Data.DBData.EntityModels;

namespace PharmacyApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Администратор")]
    [ApiController]
    public class AuthenticationLoggersController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public AuthenticationLoggersController(PharmacyContext context)
        {
            _context = context;
        }

        // GET: api/AuthenticationLoggers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthenticationLogger>>> GetAuthenticationLoggers()
        {
            return await _context.AuthenticationLoggers.Include("User").ToListAsync();
        }

        // GET: api/AuthenticationLoggers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthenticationLogger>> GetAuthenticationLogger(int id)
        {
            var authenticationLogger = await _context.AuthenticationLoggers.FindAsync(id);
            _context.Entry(authenticationLogger).Reference(p => p.User).Load();
            if (authenticationLogger == null)
            {
                return NotFound();
            }

            return authenticationLogger;
        }


        // PUT: api/AuthenticationLoggers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthenticationLogger(int id, AuthenticationLogger authenticationLogger)
        {
            if (id != authenticationLogger.Id)
            {
                return BadRequest();
            }

            _context.Entry(authenticationLogger).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthenticationLoggerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AuthenticationLoggers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthenticationLogger>> PostAuthenticationLogger(AuthenticationLogger authenticationLogger)
        {
            _context.AuthenticationLoggers.Add(authenticationLogger);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthenticationLogger", new { id = authenticationLogger.Id }, authenticationLogger);
        }

        // DELETE: api/AuthenticationLoggers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthenticationLogger(int id)
        {
            var authenticationLogger = await _context.AuthenticationLoggers.FindAsync(id);
            if (authenticationLogger == null)
            {
                return NotFound();
            }

            _context.AuthenticationLoggers.Remove(authenticationLogger);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthenticationLoggerExists(int id)
        {
            return _context.AuthenticationLoggers.Any(e => e.Id == id);
        }
    }
}
