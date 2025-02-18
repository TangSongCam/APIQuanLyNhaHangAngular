using APIQuanLyNhaHang.Data;
using APIQuanLyNhaHang.Model;
using Microsoft.AspNetCore.Mvc;
using APIQuanLyNhaHang.DTO;
using System.Linq;

namespace APIQuanLyNhaHang.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly QLNhaHangDbContext _context;

        public UserController(QLNhaHangDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            // Nếu Role chưa được gán thì mặc định là "User"
            user.Role = string.IsNullOrEmpty(user.Role) ? "User" : user.Role;
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Email == loginRequest.Email && u.Password == loginRequest.Password);

            if (user == null)
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng." });

            // Trả về đối tượng chứa userName, email và role (có thể mở rộng thêm nếu cần)
            return Ok(new { userName = user.Username, email = user.Email, role = user.Role });
        }
    }
}
