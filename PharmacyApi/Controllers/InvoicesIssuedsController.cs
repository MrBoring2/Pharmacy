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
    public class InvoicesIssuedsController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public InvoicesIssuedsController(PharmacyContext context)
        {
            _context = context;
        }

        // GET: api/InvoicesIssueds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoicesIssued>>> GetInvoicesIssueds()
        {
            return await _context.InvoicesIssueds.ToListAsync();
        }

        // GET: api/InvoicesIssueds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoicesIssued>> GetInvoicesIssued(int id)
        {
            var invoicesIssued = await _context.InvoicesIssueds.FindAsync(id);

            if (invoicesIssued == null)
            {
                return NotFound();
            }

            return invoicesIssued;
        }

        // PUT: api/InvoicesIssueds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoicesIssued(int id, InvoicesIssued invoicesIssued)
        {
            if (id != invoicesIssued.InsuranceCompanyId)
            {
                return BadRequest();
            }

            _context.Entry(invoicesIssued).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoicesIssuedExists(id))
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

        // POST: api/InvoicesIssueds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InvoicesIssued>> PostInvoicesIssued(InvoicesIssued invoicesIssued)
        {
            invoicesIssued.InsuranceCompany = _context.InsuranceСompanies.Find(invoicesIssued.InsuranceCompanyId);
            _context.InvoicesIssueds.Add(invoicesIssued);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                if (InvoicesIssuedExists(invoicesIssued.InsuranceCompanyId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetInvoicesIssued", new { id = invoicesIssued.InsuranceCompanyId }, invoicesIssued);
        }

        // DELETE: api/InvoicesIssueds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoicesIssued(int id)
        {
            var invoicesIssued = await _context.InvoicesIssueds.FindAsync(id);
            if (invoicesIssued == null)
            {
                return NotFound();
            }

            _context.InvoicesIssueds.Remove(invoicesIssued);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InvoicesIssuedExists(int id)
        {
            return _context.InvoicesIssueds.Any(e => e.InsuranceCompanyId == id);
        }
    }
}
