using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyApi.Data.DBData;
using PharmacyApi.Data.DBData.EntityModels;

namespace PharmacyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaboratoryServicesController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public LaboratoryServicesController(PharmacyContext context)
        {
            _context = context;
        }

        // GET: api/LaboratiryServices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LaboratoryService>>> GetLaboratoryServices()
        {
            return await _context.LaboratoryServices.ToListAsync();
        }

        // GET: api/LaboratiryServices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LaboratoryService>> GetLaboratoryService(int id)
        {
            var laboratiryService = await _context.LaboratoryServices.FindAsync(id);

            if (laboratiryService == null)
            {
                return NotFound();
            }

            return laboratiryService;
        }

        [HttpGet("getByName/{name}")]
        public async Task<ActionResult<LaboratoryService>> GetLaboratoryService(string name)
        {
            var laboratiryService = await _context.LaboratoryServices.FirstOrDefaultAsync(p => p.Name.Equals(name));

            if (laboratiryService == null)
            {
                return NotFound();
            }

            return laboratiryService;
        }

        // PUT: api/LaboratiryServices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLaboratoryService(int id, LaboratoryService laboratiryService)
        {
            if (id != laboratiryService.Code)
            {
                return BadRequest();
            }

            _context.Entry(laboratiryService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LaboratoryServiceExists(id))
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

        // POST: api/LaboratiryServices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LaboratoryService>> PostLaboratoryService(LaboratoryService laboratiryService)
        {
            _context.LaboratoryServices.Add(laboratiryService);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LaboratoryServiceExists(laboratiryService.Code))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLaboratiryService", new { id = laboratiryService.Code }, laboratiryService);
        }

        // DELETE: api/LaboratiryServices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLaboratoryService(int id)
        {
            var laboratiryService = await _context.LaboratoryServices.FindAsync(id);
            if (laboratiryService == null)
            {
                return NotFound();
            }

            _context.LaboratoryServices.Remove(laboratiryService);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LaboratoryServiceExists(int id)
        {
            return _context.LaboratoryServices.Any(e => e.Code == id);
        }
    }
}
