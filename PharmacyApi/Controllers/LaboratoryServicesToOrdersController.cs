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
    public class LaboratoryServicesToOrdersController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public LaboratoryServicesToOrdersController(PharmacyContext context)
        {
            _context = context;
        }

        // GET: api/LaboratoryServicesToOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LaboratoryServicesToOrder>>> GetLaboratoryServicesToOrders()
        {
            return await _context.LaboratoryServicesToOrders.Include(p=>p.LaboratoryService).ToListAsync();
        }

        // GET: api/LaboratoryServicesToOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LaboratoryServicesToOrder>> GetLaboratoryServicesToOrder(int id)
        {
            var laboratoryServicesToOrder = await _context.LaboratoryServicesToOrders.FindAsync(id);

            if (laboratoryServicesToOrder == null)
            {
                return NotFound();
            }

            return laboratoryServicesToOrder;
        }

        [HttpGet("notCompleted")]
        public async Task<ActionResult<IEnumerable<LaboratoryServicesToOrder>>> GetNotCompletedLaboratoryServicesToOrder(int analizerId)
        {
            var allServiceOrders = await _context.LaboratoryServicesToOrders
                .Include(p => p.LaboratoryService)
                .Include(p => p.Order)
                .Where(p => p.Status.Equals("Not completed"))
                .ToListAsync();

            var analizerServices = await _context.LaboratoryServiceToAnalizers
                .Where(p => p.AnalizerId == analizerId)
                .ToListAsync();

            var finalServiceOrders = new List<LaboratoryServicesToOrder>();

            foreach (var serviceOrder in allServiceOrders)
            {
                foreach (var analzierService in analizerServices)
                {
                    if (analzierService.LaboratoryServiceId == serviceOrder.LaboratoryServiceId)
                    {
                        finalServiceOrders.Add(serviceOrder);
                        break;
                    }
                }
            }

            return finalServiceOrders;
        }



        // PUT: api/LaboratoryServicesToOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLaboratoryServicesToOrder(int id, LaboratoryServicesToOrder laboratoryServicesToOrder)
        {
            if (id != laboratoryServicesToOrder.LaboratoryServiceId)
            {
                return BadRequest();
            }

            _context.Entry(laboratoryServicesToOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LaboratoryServicesToOrderExists(id))
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



        // DELETE: api/LaboratoryServicesToOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLaboratoryServicesToOrder(int id)
        {
            var laboratoryServicesToOrder = await _context.LaboratoryServicesToOrders.FindAsync(id);
            if (laboratoryServicesToOrder == null)
            {
                return NotFound();
            }

            _context.LaboratoryServicesToOrders.Remove(laboratoryServicesToOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LaboratoryServicesToOrderExists(int id)
        {
            return _context.LaboratoryServicesToOrders.Any(e => e.OrderId == id);
        }
    }
}
