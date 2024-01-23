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
    public class RestaurantController : ControllerBase
    {
        public RestaurantController(AppDbContext db)
        {
            _db = db;
        }
        private readonly AppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> GetAllRestaurants()
        {
            try
            {
                var rest = await _db.restaurant.ToListAsync();
                return Ok(rest);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting restaurants", Error = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddRestaurant([FromBody] Restaurant restaurant)
        {
            try
            {
                // Validation
                if (string.IsNullOrEmpty(restaurant.name) || string.IsNullOrEmpty(restaurant.address) || string.IsNullOrEmpty(restaurant.phone))
                {
                    return BadRequest(new { Message = "Name, Address, and Phone cannot be null or empty." });
                }

                Restaurant rest = new Restaurant { name = restaurant.name, address = restaurant.address, phone = restaurant.phone, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
                await _db.restaurant.AddAsync(rest);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Restaurant created successfully", Restaurant = rest });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error creating restaurant", Error = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurant(int id, [FromBody] Restaurant restaurant)
        {
            try
            {
                var rest = await _db.restaurant.SingleOrDefaultAsync(x => x.Id == id);

                if (rest == null)
                {
                    return NotFound($"Restaurant with ID {id} not found");
                }

                // Update only if the properties are not null or empty
                if (!string.IsNullOrEmpty(restaurant.name))
                {
                    rest.name = restaurant.name;
                }

                if (!string.IsNullOrEmpty(restaurant.address))
                {
                    rest.address = restaurant.address;
                }

                if (!string.IsNullOrEmpty(restaurant.phone))
                {
                    rest.phone = restaurant.phone;
                }

                await _db.SaveChangesAsync();

                return Ok(new { Message = "Restaurant updated successfully", Restaurant = rest });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error updating restaurant", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveRestaurant(int id)
        {
            try
            {
                var rest = await _db.restaurant.SingleOrDefaultAsync(x => x.Id == id);
                if (rest == null)
                {
                    return NotFound($"restaurant Id {id} not exists ");
                }
                _db.restaurant.Remove(rest);
                _db.SaveChanges();
                return Ok(new { Message = "Restaurant deleted successfully", Restaurant = rest });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error deleting restaurant", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurantById(int id)
        {
            try
            {
                var rest = await _db.restaurant.SingleOrDefaultAsync(x => x.Id == id);
                if (rest == null)
                {
                    return NotFound($"Restaurant Id {id} not exists ");
                }
                return Ok(rest);

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting restaurant", Error = ex.Message });
            }
        }


    }
}
