using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ProjectUI.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;

            // HttpClient kullanarak Web API'ları tüketmek için bir istemci oluşturuyoruz.
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44394/api/Auth"); // AuthController'ın bulunduğu URL'yi girin.
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserForLoginDto model)
        {
           

            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                // Token'i Cookie ya da Session'a ekleyerek kullanıcının oturum açtığını belirleyebilirsiniz.
                // Örnek olarak, Cookie kullanımı:
                var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, model.Email) // İsteğe bağlı, ekstra bilgileri burada ekleyebilirsiniz
            };

                var claimsIdentity = new ClaimsIdentity(claims, "Token");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);


                HttpContext.Response.Cookies.Append("access_token", token, new CookieOptions { HttpOnly = true });
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserForRegisterDto model)
        {
            //model.Password = HashPassword(model.Password);

            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User registration failed.");
                return View();
            }
        }

        //private string HashPassword(string password)
        //{
        //    using (var hmac = new HMACSHA512())
        //    {
        //        var passwordSalt = hmac.Key;
        //        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        //        return Convert.ToBase64String(passwordHash);
        //    }
        //}
        
    }
}
