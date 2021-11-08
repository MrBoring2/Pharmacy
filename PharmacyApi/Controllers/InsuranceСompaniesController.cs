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
    public class InsuranceСompaniesController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public InsuranceСompaniesController(PharmacyContext context)
        {
            _context = context;
        }

        // GET: api/InsuranceСompanies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InsuranceСompany>>> GetInsuranceСompanies()
        {
            return await _context.InsuranceСompanies.ToListAsync();
        }

        // GET: api/InsuranceСompanies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InsuranceСompany>> GetInsuranceСompany(int id)
        {
            var insuranceСompany = await _context.InsuranceСompanies.FindAsync(id);

            if (insuranceСompany == null)
            {
                return NotFound();
            }

            return insuranceСompany;
        }
        

        // PUT: api/InsuranceСompanies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInsuranceСompany(int id, InsuranceСompany insuranceСompany)
        {
            if (id != insuranceСompany.Id)
            {
                return BadRequest();
            }

            _context.Entry(insuranceСompany).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsuranceСompanyExists(id))
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

        // POST: api/InsuranceСompanies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InsuranceСompany>> PostInsuranceСompany(InsuranceСompany insuranceСompany)
        {
            _context.InsuranceСompanies.Add(insuranceСompany);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (InsuranceСompanyExists(insuranceСompany.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetInsuranceСompany", new { id = insuranceСompany.Id }, insuranceСompany);
        }

        // DELETE: api/InsuranceСompanies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInsuranceСompany(int id)
        {
            var insuranceСompany = await _context.InsuranceСompanies.FindAsync(id);
            if (insuranceСompany == null)
            {
                return NotFound();
            }

            _context.InsuranceСompanies.Remove(insuranceСompany);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InsuranceСompanyExists(int id)
        {
            return _context.InsuranceСompanies.Any(e => e.Id == id);
        }
    }
}
