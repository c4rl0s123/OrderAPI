using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderAPI.Models.AppContext _context;

        public OrdersController(OrderAPI.Models.AppContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder(string id)
        {
            var order = await _context.Orders.Where(o => o.OrderNumber.Equals(id)).ToListAsync();

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }


        [HttpGet("{customerId}/{startDate}/{endDate?}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrderByDates(long customerId, string startDate, string endDate)
        {
            DateTimeOffset start = DateTimeOffset.Parse(startDate);
            if (endDate != null)
            {
                DateTimeOffset end = DateTimeOffset.Parse(endDate);
                var order = await _context.Orders.Where(o => (o.CreatedDate >= start && o.CreatedDate <= end)).Where(o => o.CustomerId == customerId).ToListAsync();

                if (order == null)
                {
                    return NotFound();
                }
                return order;
            }
            else
            {
                var order = await _context.Orders.Where(o => o.CreatedDate >= start).Where(o => o.CustomerId == customerId).ToListAsync();

                if (order == null)
                {
                    return NotFound();
                }
                return order;
            }
            
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(long id, Order order)
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

        [HttpPut("updateTotal/{id}/{total}")]
        public async Task<IActionResult> PutOrderTotal(long id, decimal total)
        {
            var order = new Order() { Id = id, Total = total };
            _context.Orders.Attach(order);
            _context.Entry(order).Property(o => o.Total).IsModified = true;

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

        [HttpPut("updateStatus/{id}/{status}")]
        public async Task<IActionResult> PutOrderStatus(long id, string status)
        {
            var order = new Order() { Id = id, Status = status };
            _context.Orders.Attach(order);
            _context.Entry(order).Property(o => o.Status).IsModified = true;

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

        [HttpPut("updateOrderNumber/{id}/{orderNumber}")]
        public async Task<IActionResult> PutOrderNumber(long id, string orderNumber)
        {
            var order = new Order() { Id = id, OrderNumber = orderNumber };
            _context.Orders.Attach(order);
            _context.Entry(order).Property(o => o.OrderNumber).IsModified = true;

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
            _context.Orders.Add(order);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderExists(order.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(long id)
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

        private bool OrderExists(long id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
