using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mooc.Core.WrapperResult;
using Mooc.Model.DBContext;
using Mooc.Model.Entity;
using Mooc.Shared.SharedConfig;
using Sprache;
using Microsoft.Extensions.Options;
using Mooc.Application.Contracts.Admin.Dto.Login;
using Mooc.Core.ExceptionHandling;

namespace MoocWebApi.Controllers.Admin
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.AdminService))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfiguration _configuration;
        private readonly IOptions<JwtSettingConfig> _settingConfig;
        private readonly IRoleService _roleService;
        public AuthController(
            IAuthenticationService authenticationService,
            IConfiguration configuration,
            IOptions<JwtSettingConfig> settingConfig,
            IRoleService roleService
        )
        {
            _authenticationService = authenticationService;
            _configuration = configuration;
            _settingConfig = settingConfig;
            _roleService = roleService;
        }
        /// <summary>
        /// User login 
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>

        //POST: api/authcontroller/login
        [HttpPost]
        public async Task<LoginResponseDto> Login([FromBody] LoginRequestDto loginRequest)
        {
            LoginResponseDto loginResponseDto = new LoginResponseDto();
            var user = await _authenticationService.ValidateUserAsync(
                loginRequest.UserName,
                loginRequest.Password
            );

            if (user == null)
            {
                ExThrow.Throw("user credentials incorrect", "user credentials incorrect");
            }

            var token = GenerateJwtToken(user);
            loginResponseDto.Token = token;
            loginResponseDto.Permissions = await _roleService.GetPermissionsbyUserIdAsync(user.Id);

            return loginResponseDto;
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._settingConfig.Value.SecurityKey));
         
           var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
   
            var token = new JwtSecurityToken(
                issuer: this._settingConfig.Value.Issuer ,
                audience: this._settingConfig.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(this._settingConfig.Value.ExpireSeconds),
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