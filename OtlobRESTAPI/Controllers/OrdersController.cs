using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OtlobRESTAPI.Data;
using OtlobRESTAPI.Data.Models;

namespace OtlobRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        public OrdersController(AppDbContext db)
        {
            _db = db;
        }
        private readonly AppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orde = await _db.orders.ToListAsync();
                return Ok(orde);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting orders", Error = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] Orders orders)
        {
            try
            {
                // Validation
                if (orders.userId == 0 || orders.restaurantId == 0 || orders.statusId == 0 || orders.totalAmount == 0)
                {
                    return BadRequest(new { Message = "userId, restaurantId, statusId, and totalAmount cannot be null or empty." });
                }

                Orders orde = new Orders { userId = orders.userId, restaurantId = orders.restaurantId, statusId = orders.statusId, totalAmount = orders.totalAmount, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
                await _db.orders.AddAsync(orde);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Order created successfully", Orders = orde });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error creating order", Error = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Orders orders)
        {
            try
            {
                var orde = await _db.orders.SingleOrDefaultAsync(x => x.Id == id);

                if (orde == null)
                {
                    return NotFound($"Order with ID {id} not found");
                }

                // Update only if the properties are not null or empty
                if (orders.userId != 0)
                {
                    orde.userId = orders.userId;
                }

                if (orders.restaurantId != 0)
                {
                    orde.restaurantId = orders.restaurantId;
                }

                if (orders.statusId != 0)
                {
                    orde.statusId = orders.statusId;
                }

                if (orders.totalAmount != 0)
                {
                    orde.totalAmount = orders.totalAmount;
                }

                await _db.SaveChangesAsync();

                return Ok(new { Message = "Order updated successfully", Orders = orde });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error updating order", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveOrder(int id)
        {
            try
            {
                var orde = await _db.orders.SingleOrDefaultAsync(x => x.Id == id);
                if (orde == null)
                {
                    return NotFound($"Order Id {id} not exists ");
                }
                _db.orders.Remove(orde);
                _db.SaveChanges();
                return Ok(new { Message = "Order deleted successfully", Orders = orde });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error deleting order", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var orde = await _db.orders.SingleOrDefaultAsync(x => x.Id == id);
                if (orde == null)
                {
                    return NotFound($"Order Id {id} not exists ");
                }
                return Ok(orde);

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting order", Error = ex.Message });
            }
        }


    }
}
