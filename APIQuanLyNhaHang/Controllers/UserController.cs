using APIQuanLyNhaHang.Data;
using APIQuanLyNhaHang.Model;
using APIQuanLyNhaHang.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace APIQuanLyNhaHang.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly QLNhaHangDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(QLNhaHangDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
 
        [HttpPost("register")]
        public IActionResult Register(User user)
        {

            if (string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { message = "Password không được để trống." });
            }

            user.Role = string.IsNullOrEmpty(user.Role) ? "User" : user.Role;
            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, user.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng." });
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == loginRequest.Email);
            if (user == null)
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng." });

            if (string.IsNullOrEmpty(user.Password))
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng." });
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password!, loginRequest.Password!);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng." });

            var token = GenerateJwtToken(user);
            return Ok(new { token = token, userName = user.Username!, email = user.Email!, role = user.Role! });
        }


        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JWT");

            var keyString = jwtSettings["Key"] ?? throw new Exception("JWT Key is not configured");
            var issuer = jwtSettings["Issuer"] ?? throw new Exception("JWT Issuer is not configured");
            var audience = jwtSettings["Audience"] ?? throw new Exception("JWT Audience is not configured");
            var expiresInMinutesStr = jwtSettings["ExpiresInMinutes"] ?? "60";
            double expiresInMinutes = double.Parse(expiresInMinutesStr);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim("userName", user.Username!),
                new Claim("role", user.Role!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiresInMinutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
