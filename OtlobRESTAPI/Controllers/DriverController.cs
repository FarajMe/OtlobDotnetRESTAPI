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
    public class DriverController : ControllerBase
    {
        public DriverController(AppDbContext db)
        {
            _db = db;
        }
        private readonly AppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> GetAllDrivers()
        {
            try
            {
                var driv = await _db.driver.ToListAsync();
                return Ok(driv);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting drivers", Error = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddDriver([FromBody] Driver driver)
        {
            try
            {
                // Validation
                if (string.IsNullOrEmpty(driver.FirstName) || string.IsNullOrEmpty(driver.LastName) || string.IsNullOrEmpty(driver.Email) || string.IsNullOrEmpty(driver.PhoneNumber))
                {
                    return BadRequest(new { Message = "FirstName, LastName,  Email and PhoneNumber cannot be null or empty." });
                }

                Driver driv = new Driver { FirstName = driver.FirstName, LastName = driver.LastName, Email = driver.Email, PhoneNumber = driver.PhoneNumber, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
                await _db.driver.AddAsync(driv);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Driver created successfully", Driver = driv });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error creating driver", Error = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDriver(int id, [FromBody] Driver driver)
        {
            try
            {
                var driv = await _db.driver.SingleOrDefaultAsync(x => x.Id == id);

                if (driv == null)
                {
                    return NotFound($"Driver with ID {id} not found");
                }

                // Update only if the properties are not null or empty
                if (!string.IsNullOrEmpty(driver.FirstName))
                {
                    driv.FirstName = driver.FirstName;
                }

                if (!string.IsNullOrEmpty(driver.LastName))
                {
                    driv.LastName = driver.LastName;
                }

                if (!string.IsNullOrEmpty(driver.Email))
                {
                    driv.Email = driver.Email;
                }

                if (!string.IsNullOrEmpty(driver.PhoneNumber))
                {
                    driv.PhoneNumber = driver.PhoneNumber;
                }

                await _db.SaveChangesAsync();

                return Ok(new { Message = "Driver updated successfully", Driver = driv });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error updating driver", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveDriver(int id)
        {
            try
            {
                var driv = await _db.driver.SingleOrDefaultAsync(x => x.Id == id);
                if (driv == null)
                {
                    return NotFound($"Driver Id {id} not exists ");
                }
                _db.driver.Remove(driv);
                _db.SaveChanges();
                return Ok(new { Message = "Driver deleted successfully", Driver = driv });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error deleting driver", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriverById(int id)
        {
            try
            {
                var driv = await _db.driver.SingleOrDefaultAsync(x => x.Id == id);
                if (driv == null)
                {
                    return NotFound($"Driver Id {id} not exists ");
                }
                return Ok(driv);

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting driver", Error = ex.Message });
            }
        }


    }
}
