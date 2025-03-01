using APIQuanLyNhaHang.Data;
using APIQuanLyNhaHang.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APIQuanLyNhaHang.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : Controller
    {
        private readonly QLNhaHangDbContext _context;

        public MenuController(QLNhaHangDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetMenu()
        {
            var menuItems = _context.MenuItems.ToList();
            return Ok(menuItems);
        }

        [HttpGet("{id}")]
        public IActionResult GetMenuItem(int id)
        {
            var item = _context.MenuItems.FirstOrDefault(i => i.Id == id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> AddMenuItem([FromForm] MenuItem item, [FromQuery] string role)
        {
            if (role != "Admin")
                return Forbid("Only admins can add menu items.");

            if (item.BookingId.HasValue)
            {
                var existingBooking = _context.Bookings.Find(item.BookingId.Value);
                if (existingBooking == null)
                    return BadRequest("BookingId không hợp lệ.");
            }

            var file = Request.Form.Files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                item.ImagePath = Path.Combine("images", uniqueFileName);
            }

            item.Id = 0;
            _context.MenuItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMenuItem), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromForm] MenuItem updatedItem)
        {
            string role = Request.Headers["Role"].ToString();
            if (role != "Admin")
                return Forbid("Only admins can update menu items.");

            var item = _context.MenuItems.FirstOrDefault(i => i.Id == id);
            if (item == null)
                return NotFound();

            item.Name = updatedItem.Name;
            item.Description = updatedItem.Description;
            item.Price = updatedItem.Price;
            item.Category = updatedItem.Category;
            item.BookingId = updatedItem.BookingId;

            var file = Request.Form.Files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {
                if (!string.IsNullOrEmpty(item.ImagePath))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", item.ImagePath);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                item.ImagePath = Path.Combine("images", uniqueFileName);
            }

            await _context.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id, [FromQuery] string role)
        {
            if (role != "Admin")
                return Forbid("Only admins can delete menu items.");

            var item = _context.MenuItems.FirstOrDefault(i => i.Id == id);
            if (item == null)
                return NotFound();

            if (!string.IsNullOrEmpty(item.ImagePath))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", item.ImagePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

