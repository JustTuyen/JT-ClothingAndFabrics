using clothing_be.Data;
using clothing_be.DTO.auth;
using clothing_be.Models.customers;
using clothing_be.Models.trading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace clothing_be.Controllers.auths
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly MyDbContextApplication _context;
        private readonly IConfiguration _configuration;
        public AuthController(MyDbContextApplication context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string passwordHash, string storeHash)
        {
            return HashPassword(passwordHash) == storeHash;
        }

        private string GenerateJwtToken(UserModel user)
        {
            var jwtSetting = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var token = new JwtSecurityToken
            (
                issuer: jwtSetting["Issuer"],
                audience: jwtSetting["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSetting["ExpiryMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO request)
        {
            if (request == null) return BadRequest("Dto or request data is missing");
            if (!ModelState.IsValid) return BadRequest(ModelState);


            if (request == null || string.IsNullOrWhiteSpace(request.Password)){
                return BadRequest("Username and password are required.");
            }

            var user = await _context.Users.Include(u => u.Status).FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null) { return BadRequest("No user with this email"); }
            if (user.Status.Name != "Active") { return BadRequest("Tài khoản đã bị khóa hoặc chưa kích hoạt"); }
            if (!VerifyPassword(request.Password, user.Password)) { return BadRequest("Mistmathed password"); }

            var token = GenerateJwtToken(user);
            return Ok(new
            {
                token,
                id = user.Id,
                role = user.Role,
                email = user.Email,
            });
        }

        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromForm] UpdatePasswordDTO request)
        {
            if (request == null) return BadRequest("Dto or request data is missing");
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var userid = User.FindFirst("userId")?.Value;
            if (!int.TryParse(userid, out int currentId))
            {
                return Unauthorized("UserId is not wrong or not found!");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == currentId);
            if (user == null) return NotFound("User không tồn tại.");

            if (!VerifyPassword(request.CurrentPassword, user.Password))
                return BadRequest("Mật khẩu hiện tại không đúng.");

            if (VerifyPassword(request.NewPassword, user.Password))
                return BadRequest("Mật khẩu mới không được trùng với mật khẩu cũ.");

            string hashedPsswd = HashPassword(request.NewPassword);
            user.Password = hashedPsswd;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
            

        }
        
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()

        {
            var userid = User.FindFirst("userId")?.Value;
            if (!int.TryParse(userid, out int currentId))
            {
                return Unauthorized("UserId is not wrong or not found!");
            }

            var profile = await _context.Users
                .Where(u => u.Id == currentId)
                .Select(u => new ProfileDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Gender = u.Gender,
                    BirthDay = u.BirthDay,
                    UpdatedAt = u.UpdatedAt
                }).FirstOrDefaultAsync();

            if (profile == null) { return BadRequest("User is not found!"); }
            return Ok(profile);
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDTO request)
        {
            if (request == null) return BadRequest("Dto or request data is missing");
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var userid = User.FindFirst("userId")?.Value;
            if (!int.TryParse(userid, out int currentId))
            {
                return Unauthorized("UserId is not wrong or not found!");
            }

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest("Email đã được sử dụng.");

            if (await _context.Users.AnyAsync(u => u.PhoneNumber == request.PhoneNumber))
                return BadRequest("PhoneNumber đã được sử dụng.");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == currentId);
            if (user == null) return NotFound("User không tồn tại.");


            if (request.FirstName != null) user.FirstName = request.FirstName;
            if (request.LastName != null) user.LastName = request.LastName;
            if (request.Email != null) user.Email = request.Email;
            if (request.PhoneNumber != null) user.PhoneNumber = request.PhoneNumber;
            if (request.Gender != null) user.Gender = request.Gender;
            if (request.BirthDay.HasValue) user.BirthDay = request.BirthDay.Value;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();

        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO request)
        {
            if(request == null) return BadRequest("Dto or request data is missing");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (await _context.Users.AnyAsync(u => u.Email == request.Email)) { return BadRequest("email already used"); }

            var status = await _context.Statuses.Where(i => i.Type == "Users" && i.Name == "Active").FirstOrDefaultAsync();
            if (status == null) return BadRequest("Status for user is not foudn???");
            
            string hashedPsswd = HashPassword(request.Password);
            var user = new UserModel
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = hashedPsswd,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                StatusId = status.Id
                
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var cart = new CartModel
            {
                UserId = user.Id
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return Ok(new { id = user.Id, email = user.Email, message = "Đăng ký thành công." });
        }
    }
    
}
