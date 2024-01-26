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
    public class OrderItemController : ControllerBase
    {
        public OrderItemController(AppDbContext db)
        {
            _db = db;
        }
        private readonly AppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> GetAllOrderItems()
        {
            try
            {
                var orde = await _db.orderitem.ToListAsync();
                return Ok(orde);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting orderitems", Error = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddOrderItem([FromBody] OrderItem orderitem)
        {
            try
            {
                // Validation
                if (orderitem.orderId == 0 || orderitem.menuItemId == 0 || orderitem.quantity == 0 || orderitem.subtotal == 0)
                {
                    return BadRequest(new { Message = "orderId, menuItemId, quantity, and subtotal cannot be null or empty." });
                }

                OrderItem orde = new OrderItem { orderId = orderitem.orderId, menuItemId = orderitem.menuItemId, quantity = orderitem.quantity, subtotal = orderitem.subtotal};
                await _db.orderitem.AddAsync(orde);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "OrderItem created successfully", OrderItem = orde });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error creating orderitem", Error = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, [FromBody] OrderItem orderitem)
        {
            try
            {
                var orde = await _db.orderitem.SingleOrDefaultAsync(x => x.Id == id);

                if (orde == null)
                {
                    return NotFound($"OrderItem with ID {id} not found");
                }

                // Update only if the properties are not null or empty
                if (orderitem.orderId != 0)
                {
                    orde.orderId = orderitem.orderId;
                }

                if (orderitem.menuItemId != 0)
                {
                    orde.menuItemId = orderitem.menuItemId;
                }

                if (orderitem.quantity != 0)
                {
                    orde.quantity = orderitem.quantity;
                }

                if (orderitem.subtotal != 0)
                {
                    orde.subtotal = orderitem.subtotal;
                }

                await _db.SaveChangesAsync();

                return Ok(new { Message = "OrderItem updated successfully", OrderItem = orde });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error updating orderitem", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveOrderItem(int id)
        {
            try
            {
                var orde = await _db.orderitem.SingleOrDefaultAsync(x => x.Id == id);
                if (orde == null)
                {
                    return NotFound($"OrderItem Id {id} not exists ");
                }
                _db.orderitem.Remove(orde);
                _db.SaveChanges();
                return Ok(new { Message = "OrderItem deleted successfully", OrderItem = orde });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error deleting orderitem", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItemById(int id)
        {
            try
            {
                var orde = await _db.orderitem.SingleOrDefaultAsync(x => x.Id == id);
                if (orde == null)
                {
                    return NotFound($"OrderItem Id {id} not exists ");
                }
                return Ok(orde);

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting orderitem", Error = ex.Message });
            }
        }


    }
}
