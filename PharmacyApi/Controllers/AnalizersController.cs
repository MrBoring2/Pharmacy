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
    public class AnalizersController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public AnalizersController(PharmacyContext context)
        {
            _context = context;
        }

        // GET: api/Analizers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Analizer>>> GetAnalizers()
        {
            return await _context.Analizers.Include(p => p.LaboratoryServiceToAnalizers).Include("LaboratoryServiceToAnalizers.LaboratoryService").ToListAsync();
        }

        // GET: api/Analizers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Analizer>> GetAnalizer(int id)
        {
            var analizer = await _context.Analizers.FindAsync(id);

            if (analizer == null)
            {
                return NotFound();
            }

            return analizer;
        }

        // PUT: api/Analizers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnalizer(int id, Analizer analizer)
        {
            if (id != analizer.Id)
            {
                return BadRequest();
            }

            _context.Entry(analizer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnalizerExists(id))
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

        // POST: api/Analizers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Analizer>> PostAnalizer(Analizer analizer)
        {
            _context.Analizers.Add(analizer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnalizer", new { id = analizer.Id }, analizer);
        }

        // DELETE: api/Analizers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnalizer(int id)
        {
            var analizer = await _context.Analizers.FindAsync(id);
            if (analizer == null)
            {
                return NotFound();
            }

            _context.Analizers.Remove(analizer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnalizerExists(int id)
        {
            return _context.Analizers.Any(e => e.Id == id);
        }
    }
}
