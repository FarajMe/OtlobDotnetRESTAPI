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
    public class VehicleController : ControllerBase
    {
        public VehicleController(AppDbContext db)
        {
            _db = db;
        }
        private readonly AppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> GetAllVehicles()
        {
            try
            {
                var vehi = await _db.vehicle.ToListAsync();
                return Ok(vehi);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting vehicles", Error = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddVehicle([FromBody] Vehicle vehicle)
        {
            try
            {
                // Validation
                if (string.IsNullOrEmpty(vehicle.vehicleType) || string.IsNullOrEmpty(vehicle.plateNumber) || string.IsNullOrEmpty(vehicle.model) || string.IsNullOrEmpty(vehicle.color) || vehicle.vehicleTypeId == 0)
                {
                    return BadRequest(new { Message = "vehicleType, plateNumber,  model, vehicleTypeId and color cannot be null or empty." });
                }

                Vehicle vehi = new Vehicle { vehicleType = vehicle.vehicleType, plateNumber = vehicle.plateNumber, model = vehicle.model, color = vehicle.color, vehicleTypeId = vehicle.vehicleTypeId, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
                await _db.vehicle.AddAsync(vehi);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Vehicle created successfully", Vehicle = vehi });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error creating vehicle", Error = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] Vehicle vehicle)
        {
            try
            {
                var vehi = await _db.vehicle.SingleOrDefaultAsync(x => x.Id == id);

                if (vehi == null)
                {
                    return NotFound($"Vehicle with ID {id} not found");
                }

                // Update only if the properties are not null or empty
                if (!string.IsNullOrEmpty(vehicle.vehicleType))
                {
                    vehi.vehicleType = vehicle.vehicleType;
                }

                if (!string.IsNullOrEmpty(vehicle.plateNumber))
                {
                    vehi.plateNumber = vehicle.plateNumber;
                }

                if (!string.IsNullOrEmpty(vehicle.model))
                {
                    vehi.model = vehicle.model;
                }

                if (!string.IsNullOrEmpty(vehicle.color))
                {
                    vehi.color = vehicle.color;
                }

                if (vehicle.vehicleTypeId != 0)
                {
                    vehi.vehicleTypeId = vehicle.vehicleTypeId;
                }

                await _db.SaveChangesAsync();

                return Ok(new { Message = "Vehicle updated successfully", Vehicle = vehi });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error updating vehicle", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveVehicle(int id)
        {
            try
            {
                var vehi = await _db.vehicle.SingleOrDefaultAsync(x => x.Id == id);
                if (vehi == null)
                {
                    return NotFound($"Vehicle Id {id} not exists ");
                }
                _db.vehicle.Remove(vehi);
                _db.SaveChanges();
                return Ok(new { Message = "Vehicle deleted successfully", Vehicle = vehi });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error deleting vehicle", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            try
            {
                var vehi = await _db.vehicle.SingleOrDefaultAsync(x => x.Id == id);
                if (vehi == null)
                {
                    return NotFound($"Vehicle Id {id} not exists ");
                }
                return Ok(vehi);

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting vehicle", Error = ex.Message });
            }
        }


    }
}
