using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LibraryAPI.Controllers
{
    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // "Admin", "User", "Librarian"
    }

    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly LibraryDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthController(LibraryDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
        }

        /// <summary>
        /// Registers a new user with one-way hashed password.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { message = "Username and password are required." });
            }

            // Check if username already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == dto.Username.ToLower());
            if (existingUser != null)
            {
                return Conflict(new { message = "A user with this username already exists." });
            }

            var user = new User
            {
                Username = dto.Username,
                Email = string.IsNullOrWhiteSpace(dto.Email) ? $"{dto.Username}@library.local" : dto.Email,
                Role = string.IsNullOrWhiteSpace(dto.Role) ? "User" : dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            // Secure one-way password hashing
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "User registered successfully.",
                userId = user.UserId,
                username = user.Username,
                role = user.Role
            });
        }

        /// <summary>
        /// Authenticates user credentials and returns a signed JWT token.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { message = "Username and password are required." });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == dto.Username.ToLower());
            
            // Seed fallback for demo admin if user table was freshly created
            if (user == null && dto.Username.Equals("admin", StringComparison.OrdinalIgnoreCase) && dto.Password == "Admin123!")
            {
                user = new User
                {
                    Username = "admin",
                    Email = "admin@library.local",
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                };
                user.PasswordHash = _passwordHasher.HashPassword(user, "Admin123!");
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            // Verify password against stored hash
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var tokenString = GenerateJwt(user, out DateTime expiresAt);

            return Ok(new AuthResponseDto
            {
                Token = tokenString,
                Username = user.Username,
                Role = user.Role,
                Expiration = expiresAt
            });
        }

        private string GenerateJwt(User user, out DateTime expiresAt)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? "SuperSecretLibraryKeyForWeek4InternshipJWTTokenValidation2026!";
            var expiryMinutes = int.TryParse(_configuration["Jwt:ExpiryMinutes"], out int exp) ? exp : 120;
            expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "LibraryAPI",
                audience: _configuration["Jwt:Audience"] ?? "LibraryAppUsers",
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
