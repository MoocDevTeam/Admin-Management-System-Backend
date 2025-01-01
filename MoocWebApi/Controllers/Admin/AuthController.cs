using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mooc.Model.DBContext;
using Mooc.Model.Entity;

namespace MoocWebApi.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MoocDBContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(MoocDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //POST: api/authcontroller/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // search user
            var user = await _context.Users.SingleOrDefaultAsync(u =>
                u.UserName == request.UserName
            );

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var token = GenerateJwtToken(user);

            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtSetting:SecurityKey"])
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSetting:Issuer"],
                audience: _configuration["JwtSetting:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //POST: api/authcontroller/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == model.UserName))
            {
                return BadRequest("Username already exists.");
            }

            var user = new User
            {
                UserName = model.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }
    }
}

public class LoginRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class RegisterRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
