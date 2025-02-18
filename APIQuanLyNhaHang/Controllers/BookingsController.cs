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

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings.Include(b => b.Table).ToListAsync();
        }

        // GET: api/Bookings/5
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

        // GET: api/Bookings/5/menuitems
        [HttpGet("{id}/menuitems")]
        public IActionResult GetBookingWithMenuItems(int id)
        {
            var booking = _context.Bookings
                .Include(b => b.MenuItems) // Lấy danh sách món ăn
                .Include(b => b.Table)     // Lấy thông tin bàn
                .FirstOrDefault(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        // GET: api/Bookings/tables
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

        // POST: api/Bookings
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
            // Kiểm tra bắt buộc phải có thời gian đặt bàn
            if (booking.BookingTime == null)
            {
                return BadRequest(new { message = "Thời gian đặt bàn không được để trống!" });
            }

            // Thêm đặt bàn mới
            _context.Bookings.Add(booking);

            // Tìm bàn theo TableId và cập nhật trạng thái
            var table = await _context.Tables.FindAsync(booking.TableId);
            if (table != null)
            {
                table.Status = "Occupied"; // Cập nhật trạng thái bàn thành Occupied
                _context.Entry(table).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
        }

        // PUT: api/Bookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest(new { message = "Id không khớp!" });
            }

            // Kiểm tra đặt bàn
            var existingBooking = await _context.Bookings
                .Include(b => b.Table)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (existingBooking == null)
            {
                return NotFound(new { message = "Không tìm thấy đặt bàn!" });
            }

            // Cập nhật thông tin đặt bàn (bao gồm cả BookingTime)
            _context.Entry(existingBooking).CurrentValues.SetValues(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            // Tìm đặt bàn
            var booking = await _context.Bookings
                .Include(b => b.Table)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null)
            {
                return NotFound(new { message = "Không tìm thấy đặt bàn!" });
            }

            // Xóa đặt bàn
            _context.Bookings.Remove(booking);

            // Cập nhật trạng thái bàn thành Available nếu có
            var table = booking.Table;
            if (table != null)
            {
                table.Status = "Available";
                _context.Tables.Update(table);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
