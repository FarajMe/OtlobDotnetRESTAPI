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
    public class DeliveryController : ControllerBase
    {
        public DeliveryController(AppDbContext db)
        {
            _db = db;
        }
        private readonly AppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> GetAllDeliveries()
        {
            try
            {
                var deli = await _db.delivery.ToListAsync();
                return Ok(deli);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting deliveries", Error = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddDelivery([FromBody] Delivery delivery)
        {
            try
            {
                // Validation
                if (delivery.orderId == 0 || delivery.driverId == 0 || delivery.vehicleId == 0 || string.IsNullOrEmpty(delivery.deliveryStatus))
                {
                    return BadRequest(new { Message = "orderId, driverId, vehicleId, and deliveryStatus cannot be null or empty." });
                }

                Delivery deli = new Delivery { orderId = delivery.orderId, driverId = delivery.driverId, vehicleId = delivery.vehicleId, deliveryStatus = delivery.deliveryStatus, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
                await _db.delivery.AddAsync(deli);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Delivery created successfully", Delivery = deli });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error creating delivery", Error = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDelivery(int id, [FromBody] Delivery delivery)
        {
            try
            {
                var deli = await _db.delivery.SingleOrDefaultAsync(x => x.Id == id);

                if (deli == null)
                {
                    return NotFound($"Delivery with ID {id} not found");
                }

                // Update only if the properties are not null or empty
                if (delivery.orderId != 0)
                {
                    deli.orderId = delivery.orderId;
                }

                if (delivery.driverId != 0)
                {
                    deli.driverId = delivery.driverId;
                }

                if (delivery.vehicleId != 0)
                {
                    deli.vehicleId = delivery.vehicleId;
                }

                if (!string.IsNullOrEmpty(delivery.deliveryStatus))
                {
                    deli.deliveryStatus = delivery.deliveryStatus;
                }

                await _db.SaveChangesAsync();

                return Ok(new { Message = "Delivery updated successfully", Delivery = deli });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error updating delivery", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveDelivery(int id)
        {
            try
            {
                var deli = await _db.delivery.SingleOrDefaultAsync(x => x.Id == id);
                if (deli == null)
                {
                    return NotFound($"Delivery Id {id} not exists ");
                }
                _db.delivery.Remove(deli);
                _db.SaveChanges();
                return Ok(new { Message = "Delivery deleted successfully", Delivery = deli });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error deleting delivery", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeliveryById(int id)
        {
            try
            {
                var deli = await _db.delivery.SingleOrDefaultAsync(x => x.Id == id);
                if (deli == null)
                {
                    return NotFound($"Delivery Id {id} not exists ");
                }
                return Ok(deli);

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting delivery", Error = ex.Message });
            }
        }


    }
}
