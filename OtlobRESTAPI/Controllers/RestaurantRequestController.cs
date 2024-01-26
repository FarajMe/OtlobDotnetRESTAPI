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
    public class RestaurantRequestController : ControllerBase
    {
        public RestaurantRequestController(AppDbContext db)
        {
            _db = db;
        }
        private readonly AppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> GetAllRestaurantRequests()
        {
            try
            {
                var rest = await _db.restaurantrequest.ToListAsync();
                return Ok(rest);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting restaurantrequests", Error = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddRestaurantRequest([FromBody] RestaurantRequest restaurantrequest)
        {
            try
            {
                // Validation
                if (string.IsNullOrEmpty(restaurantrequest.address) || string.IsNullOrEmpty(restaurantrequest.floor) || string.IsNullOrEmpty(restaurantrequest.storeName) || string.IsNullOrEmpty(restaurantrequest.brandName) 
                    || string.IsNullOrEmpty(restaurantrequest.firstName) || string.IsNullOrEmpty(restaurantrequest.lastName) || string.IsNullOrEmpty(restaurantrequest.email) || string.IsNullOrEmpty(restaurantrequest.phoneNumber) || restaurantrequest.statusId == 0)
                {
                    return BadRequest(new { Message = "address, floor, storeName, brandName, firstName, lastName, email, phoneNumber and statusId cannot be null or empty." });
                }

                RestaurantRequest rest = new RestaurantRequest { address = restaurantrequest.address, floor = restaurantrequest.floor, storeName = restaurantrequest.storeName,
                    brandName = restaurantrequest.brandName, firstName = restaurantrequest.firstName, lastName = restaurantrequest.lastName, email = restaurantrequest.email,
                    phoneNumber = restaurantrequest.phoneNumber, statusId = restaurantrequest.statusId, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
                await _db.restaurantrequest.AddAsync(rest);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "RestaurantRequest created successfully", RestaurantRequest = rest });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error creating restaurantrequest", Error = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurantRequest(int id, [FromBody] RestaurantRequest restaurantrequest)
        {
            try
            {
                var rest = await _db.restaurantrequest.SingleOrDefaultAsync(x => x.Id == id);

                if (rest == null)
                {
                    return NotFound($"RestaurantRequest with ID {id} not found");
                }

                // Update only if the properties are not null or empty
                if (!string.IsNullOrEmpty(restaurantrequest.address))
                {
                    rest.address = restaurantrequest.address;
                }

                if (!string.IsNullOrEmpty(restaurantrequest.floor))
                {
                    rest.floor = restaurantrequest.floor;
                }

                if (!string.IsNullOrEmpty(restaurantrequest.storeName))
                {
                    rest.storeName = restaurantrequest.storeName;
                }

                if (!string.IsNullOrEmpty(restaurantrequest.brandName))
                {
                    rest.brandName = restaurantrequest.brandName;
                }

                if (!string.IsNullOrEmpty(restaurantrequest.firstName))
                {
                    rest.firstName = restaurantrequest.firstName;
                }

                if (!string.IsNullOrEmpty(restaurantrequest.lastName))
                {
                    rest.lastName = restaurantrequest.lastName;
                }

                if (!string.IsNullOrEmpty(restaurantrequest.email))
                {
                    rest.email = restaurantrequest.email;
                }

                if (!string.IsNullOrEmpty(restaurantrequest.phoneNumber))
                {
                    rest.phoneNumber = restaurantrequest.phoneNumber;
                }

                if (restaurantrequest.statusId != 0)
                {
                    rest.statusId = restaurantrequest.statusId;
                }

                await _db.SaveChangesAsync();

                return Ok(new { Message = "RestaurantRequest updated successfully", RestaurantRequest = rest });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error updating restaurantrequest", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveRestaurantRequest(int id)
        {
            try
            {
                var rest = await _db.restaurantrequest.SingleOrDefaultAsync(x => x.Id == id);
                if (rest == null)
                {
                    return NotFound($"restaurantrequest Id {id} not exists ");
                }
                _db.restaurantrequest.Remove(rest);
                _db.SaveChanges();
                return Ok(new { Message = "RestaurantRequest deleted successfully", RestaurantRequest = rest });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error deleting restaurantrequest", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurantRequestById(int id)
        {
            try
            {
                var rest = await _db.restaurantrequest.SingleOrDefaultAsync(x => x.Id == id);
                if (rest == null)
                {
                    return NotFound($"RestaurantRequest Id {id} not exists ");
                }
                return Ok(rest);

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting restaurantrequest", Error = ex.Message });
            }
        }


    }
}
