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
    public class LookupController : ControllerBase
    {
        public LookupController(AppDbContext db)
        {
            _db = db;
        }
        private readonly AppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> GetAllLookups()
        {
            try
            {
            var look = await _db.lookup.ToListAsync();
            return Ok(look);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting lookups", Error = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddLookup([FromBody] Lookup lookup)
        {
            try
            {
                // Validation
                if (string.IsNullOrEmpty(lookup.Value) || string.IsNullOrEmpty(lookup.ReferenceType))
                {
                    return BadRequest(new { Message = "Value and ReferenceType cannot be null or empty." });
                }

                Lookup look = new Lookup { Value = lookup.Value, ReferenceType = lookup.ReferenceType };
                await _db.lookup.AddAsync(look);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "Lookup created successfully", Lookup = look });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error creating lookup", Error = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLookup(int id, [FromBody] Lookup lookup)
        {
            try
            {
                var look = await _db.lookup.SingleOrDefaultAsync(x => x.Id == id);

                if (look == null)
                {
                    return NotFound($"Lookup with ID {id} not found");
                }

                // Update only if the properties are not null or empty
                if (!string.IsNullOrEmpty(lookup.Value))
                {
                    look.Value = lookup.Value;
                }

                if (!string.IsNullOrEmpty(lookup.ReferenceType))
                {
                    look.ReferenceType = lookup.ReferenceType;
                }

                await _db.SaveChangesAsync();

                return Ok(new { Message = "Lookup updated successfully", Lookup = look });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error updating lookup", Error = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveLookup(int id)
        {
            try
            {
                var look = await _db.lookup.SingleOrDefaultAsync(x => x.Id == id);
                if (look == null)
                {
                    return NotFound($"Lookup Id {id} not exists ");
                }
                _db.lookup.Remove(look);
                _db.SaveChanges();
                return Ok(new { Message = "Lookup deleted successfully", Lookup = look });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error deleting lookup", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLookupById(int id)
        {
            try
            {
            var look = await _db.lookup.SingleOrDefaultAsync(x => x.Id == id);
            if (look == null)
            {
                return NotFound($"Lookup Id {id} not exists ");
            }
            return Ok(look);

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting lookup", Error = ex.Message });
            }
        }


    }
}
