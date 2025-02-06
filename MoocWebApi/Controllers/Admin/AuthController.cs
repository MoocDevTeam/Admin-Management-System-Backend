using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mooc.Core.WrapperResult;
using Mooc.Model.DBContext;
using Mooc.Model.Entity;

namespace MoocWebApi.Controllers.Admin
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.AdminService))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfiguration _configuration;

        public AuthController(
            IAuthenticationService authenticationService,
            IConfiguration configuration
        )
        {
            _authenticationService = authenticationService;
            _configuration = configuration;
        }
        /// <summary>
        /// User login 
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>

        //POST: api/authcontroller/login
        [HttpPost]
        public async Task<ApiResponseResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var user = await _authenticationService.ValidateUserAsync(
                loginRequest.UserName,
                loginRequest.Password
            );

            if (user == null)
            {
                return new ApiResponseResult() { IsSuccess = false, Status = 404, Message = "Username or password is not correct, please enter again !", Time = DateTime.Now };

            }

            var token = GenerateJwtToken(user);

            return new ApiResponseResult() { IsSuccess = true, Status = 200, Message = token, Time = DateTime.Now };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };


            // get key from configuration
            var key = _configuration["JwtSetting:SecurityKey"];
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT SecurityKey is not configured.");
            }

            //var key = new SymmetricSecurityKey(
            //    Encoding.UTF8.GetBytes(_configuration["JwtSetting:SecurityKey"])
            //);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

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
        // [HttpPost("register")]
        // public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        // {
        //     if (await _context.Users.AnyAsync(u => u.UserName == model.UserName))
        //     {
        //         return BadRequest("Username already exists.");
        //     }

        //     var user = new User
        //     {
        //         UserName = model.UserName,
        //         Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
        //     };

        //     _context.Users.Add(user);
        //     await _context.SaveChangesAsync();

        //     return Ok("User registered successfully.");
        // }
    }
}