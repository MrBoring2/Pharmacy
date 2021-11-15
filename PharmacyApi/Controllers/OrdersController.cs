﻿using System;
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
    public class OrdersController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public OrdersController(PharmacyContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _context.Orders.Include(p => p.LaboratoryServicesToOrders).ToListAsync();
            foreach (var item in orders)
            {
               
                foreach (var service in item.LaboratoryServicesToOrders)
                {
                    _context.Entry(service).Reference(p => p.LaboratoryService).Load();
                }
            }

            return orders;
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Entry(order).Collection(p => p.LaboratoryServicesToOrders).Load();
            _context.Entry(order).Reference(p => p.Patient).Load();
            foreach (var service in order.LaboratoryServicesToOrders)
            {
                _context.Entry(service).Reference(p => p.LaboratoryService).Load();
            }
            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
        [HttpGet("getByDate")]
        public async Task<ActionResult<List<Order>>> GetOrdersByDateAndCompany(int insuranceCompanyId, string dateStart, string dateEnd)
        {
            var orders = await _context.Orders
                .Include(p => p.LaboratoryServicesToOrders)
                .Where(p => p.Patient.InsuranceCompanyId == insuranceCompanyId)
                .Where(p => Convert.ToDouble(p.DateOfCreation)>= Convert.ToDouble(dateStart) 
                && Convert.ToDouble(p.DateOfCreation) <= Convert.ToDouble(dateEnd)).ToListAsync();
            foreach (var item in orders)
            {
                _context.Entry(item).Reference(p => p.Patient).Load();

                foreach (var service in item.LaboratoryServicesToOrders)
                {
                    _context.Entry(service).Reference(p => p.LaboratoryService).Load();
                }
            }
            return orders;
        }


        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            for (int i = 0; i < order.LaboratoryServicesToOrders.Count(); i++)
            {
                order.LaboratoryServicesToOrders.ToList()[i].LaboratoryService = _context.LaboratoryServices.First(p => p.Code == order.LaboratoryServicesToOrders.ToList()[i].LaboratoryService.Code);
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}