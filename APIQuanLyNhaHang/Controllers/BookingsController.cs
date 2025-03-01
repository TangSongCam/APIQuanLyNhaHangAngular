using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIQuanLyNhaHang.Data;
using APIQuanLyNhaHang.Model;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace APIQuanLyNhaHang.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly QLNhaHangDbContext _context;

        public BookingsController(QLNhaHangDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings.Include(b => b.Table).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Table)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        [HttpGet("{id}/menuitems")]
        public IActionResult GetBookingWithMenuItems(int id)
        {
            var booking = _context.Bookings
                .Include(b => b.MenuItems)
                .Include(b => b.Table)
                .FirstOrDefault(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        [HttpGet("tables")]
        public IActionResult GetTables()
        {
            var tables = _context.Tables.Select(table => new
            {
                Id = table.Id,
                Status = _context.Bookings.Any(b => b.TableId == table.Id && b.BookingDate.Date == DateTime.Today)
                    ? "Occupied"
                    : "Available"
            }).ToList();

            return Ok(tables);
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
            if (booking.BookingTime == null)
            {
                return BadRequest(new { message = "Thời gian đặt bàn không được để trống!" });
            }

            _context.Bookings.Add(booking);

            var table = await _context.Tables.FindAsync(booking.TableId);
            if (table != null)
            {
                table.Status = "Occupied";
                _context.Entry(table).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest(new { message = "Id không khớp!" });
            }

            var existingBooking = await _context.Bookings
                .Include(b => b.Table)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (existingBooking == null)
            {
                return NotFound(new { message = "Không tìm thấy đặt bàn!" });
            }

            _context.Entry(existingBooking).CurrentValues.SetValues(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.MenuItems) // Lấy luôn các menu items liên quan
                .Include(b => b.Table)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null)
            {
                return NotFound(new { message = "Không tìm thấy đặt bàn!" });
            }

            // Xóa các menu items liên quan nếu có
            if (booking.MenuItems != null && booking.MenuItems.Any())
            {
                _context.MenuItems.RemoveRange(booking.MenuItems);
            }

            // Xóa booking
            _context.Bookings.Remove(booking);

            // Cập nhật trạng thái bàn nếu có
            var table = booking.Table;
            if (table != null)
            {
                table.Status = "Available";
                _context.Tables.Update(table);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa đặt bàn!", error = ex.Message });
            }

            return NoContent();
        }


        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
