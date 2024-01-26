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
    public class MenuItemController : ControllerBase
    {
        public MenuItemController(AppDbContext db)
        {
            _db = db;
        }
        private readonly AppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> GetAllMenuItems()
        {
            try
            {
                var menu = await _db.menuitem.ToListAsync();
                return Ok(menu);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting menuitems", Error = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddMenuItem([FromBody] MenuItem menuitem)
        {
            try
            {
                // Validation
                if (menuitem.restaurantId == 0 || menuitem.price == 0 || string.IsNullOrEmpty(menuitem.name) || string.IsNullOrEmpty(menuitem.description))
                {
                    return BadRequest(new { Message = "restaurantId, price, name, and description cannot be null or empty." });
                }

                MenuItem menu = new MenuItem { restaurantId = menuitem.restaurantId, price = menuitem.price, name = menuitem.name, description = menuitem.description, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
                await _db.menuitem.AddAsync(menu);
                await _db.SaveChangesAsync();

                return Ok(new { Message = "MenuItem created successfully", MenuItem = menu });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error creating menuitem", Error = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuItem menuitem)
        {
            try
            {
                var menu = await _db.menuitem.SingleOrDefaultAsync(x => x.Id == id);

                if (menu == null)
                {
                    return NotFound($"MenuItem with ID {id} not found");
                }

                // Update only if the properties are not null or empty
                if (menuitem.restaurantId != 0)
                {
                    menu.restaurantId = menuitem.restaurantId;
                }

                if (menuitem.price != 0)
                {
                    menu.price = menuitem.price;
                }

                if (!string.IsNullOrEmpty(menuitem.name))
                {
                    menu.name = menuitem.name;
                }

                if (!string.IsNullOrEmpty(menuitem.description))
                {
                    menu.description = menuitem.description;
                }

                await _db.SaveChangesAsync();

                return Ok(new { Message = "MenuItem updated successfully", MenuItem = menu });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error updating menuitem", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveMenuItem(int id)
        {
            try
            {
                var menu = await _db.menuitem.SingleOrDefaultAsync(x => x.Id == id);
                if (menu == null)
                {
                    return NotFound($"MenuItem Id {id} not exists ");
                }
                _db.menuitem.Remove(menu);
                _db.SaveChanges();
                return Ok(new { Message = "MenuItem deleted successfully", MenuItem = menu });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error deleting menuitem", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuItemById(int id)
        {
            try
            {
                var menu = await _db.menuitem.SingleOrDefaultAsync(x => x.Id == id);
                if (menu == null)
                {
                    return NotFound($"MenuItem Id {id} not exists ");
                }
                return Ok(menu);

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error getting menuitem", Error = ex.Message });
            }
        }


    }
}
