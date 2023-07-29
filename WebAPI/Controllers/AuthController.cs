using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
		private readonly IAuthService _authService;

		public AuthController(IConfiguration configuration,IAuthService authService)
        {
            _configuration = configuration;
			_authService = authService;
		}

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserForRegisterDto request)
        {
			var registerResult = _authService.Register(request);
			return Ok(registerResult);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserForLoginDto request)
        {
			var userToLogin = _authService.Login(request);

			string token = CreateToken(userToLogin);
            return Ok(token);
        }
		[HttpPost("logout")]
		public async Task<IActionResult> LogOut()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return Ok();
		}

		private string CreateToken(User user)
		{
			
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Email,user.Email)
			};

			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				issuer: "https://localhost:44394",
				audience: "https://localhost:44394",
						 claims: claims,
				expires: DateTime.Now.AddDays(1),
				notBefore: DateTime.Now,
				signingCredentials: creds
				);


			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;

		}
		[HttpGet]
		public bool ValidateToken(string token)
		{
			var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
			try
			{
				JwtSecurityTokenHandler handler = new();
				handler.ValidateToken(token, new TokenValidationParameters()
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = securityKey,
					ValidateLifetime = true,
					ValidateAudience = false,
					ValidateIssuer = false,
				}, out SecurityToken validatedToken
				);
				var jwtToken = (JwtSecurityToken)validatedToken;
				var claims = jwtToken.Claims.ToList();
				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}

	}
}
