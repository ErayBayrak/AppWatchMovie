using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        private readonly IUserService _userService;

        public LoginController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44394/api/Auth"); 
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _userService = userService;
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
               
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
            };

                var claimsIdentity = new ClaimsIdentity(claims, "Token");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);

                HttpContext.Response.Cookies.Append("access_token", token, new CookieOptions { HttpOnly = true });

                var userToCheck = _userService.GetByMail(model.Email);

                HttpContext.Session.SetString("UserMail", userToCheck.Email);
                HttpContext.Session.SetString("UserFirstName", userToCheck.FirstName);
                HttpContext.Session.SetString("UserLastName", userToCheck.LastName);
                return RedirectToAction("Index", "Movie");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {

            var response = await _httpClient.PostAsync("Auth/logout", null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Logout failed.");
                return View();
            }
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //return RedirectToAction("Login", "Login");

        }


        [HttpPost]

        public User GetByMail(string email)
        {
            var values = _userService.Get(u => u.Email == email);
            return values;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserForRegisterDto model)
        {
            
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
    }
}
