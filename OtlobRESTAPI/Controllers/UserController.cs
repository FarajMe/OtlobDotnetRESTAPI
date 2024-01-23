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
    public class UserController : ControllerBase
    {
        public UserController(AppDbContext db)
        {
            _db = db;
        }
        private readonly AppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var usr = await _db.user.ToListAsync();
                return Ok(usr);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting users", Error = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            try
            {
                // Validation
                if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                {
                    return BadRequest(new { Message = "FirstName, LastName,  Email and Password cannot be null or empty." });
                }

                User usr = new User { FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, Password = user.Password, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
                await _db.user.AddAsync(usr);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "User created successfully", User = usr });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error creating user", Error = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            try
            {
                var usr = await _db.user.SingleOrDefaultAsync(x => x.Id == id);

                if (usr == null)
                {
                    return NotFound($"User with ID {id} not found");
                }

                // Update only if the properties are not null or empty
                if (!string.IsNullOrEmpty(user.FirstName))
                {
                    usr.FirstName = user.FirstName;
                }

                if (!string.IsNullOrEmpty(user.LastName))
                {
                    usr.LastName = user.LastName;
                }

                if (!string.IsNullOrEmpty(user.Email))
                {
                    usr.Email = user.Email;
                }

                if (!string.IsNullOrEmpty(user.Password))
                {
                    usr.Password = user.Password;
                }

                await _db.SaveChangesAsync();

                return Ok(new { Message = "User updated successfully", User = usr });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error updating user", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUser(int id)
        {
            try
            {
                var usr = await _db.user.SingleOrDefaultAsync(x => x.Id == id);
                if (usr == null)
                {
                    return NotFound($"user Id {id} not exists ");
                }
                _db.user.Remove(usr);
                _db.SaveChanges();
                return Ok(new { Message = "User deleted successfully", User = usr });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error deleting User", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var usr = await _db.user.SingleOrDefaultAsync(x => x.Id == id);
                if (usr == null)
                {
                    return NotFound($"User Id {id} not exists ");
                }
                return Ok(usr);

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting User", Error = ex.Message });
            }
        }


    }
}
