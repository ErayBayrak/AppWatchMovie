using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace ProjectUI.Controllers
{
    public class LoginController : Controller
    {
       private readonly IAuthService _authService;
        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Index()
        {

            return View();
        }
        //      [HttpPost]
        //public async Task<IActionResult> Index(User user)
        //{

        //          //_authService.Login(user);
        //	return RedirectToAction("Index","Home");
        //}

        [HttpPost]
        public async Task<ActionResult> Login(UserForLoginDto dto)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://your-web-api-url.com/"); // Web API URL
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Örnek olarak, kullanıcı adı ve şifreyi modelden alıp Web API'ye post edelim
                    HttpResponseMessage response = await client.PostAsJsonAsync("api/auth/login", dto);

                    if (response.IsSuccessStatusCode)
                    {
                        // Web API'den dönen JWT'yi alalım
                        var token = await response.Content.ReadAsAsync<string>();

                        
                        HttpContext.Session.SetString("AccessToken", token);
                       

                        return RedirectToAction("Index", "Home"); 
                    }
                    else
                    {
                        // Giriş başarısız
                        ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
                    }
                }
            }

            // Hata durumunda ya da ilk giriş sayfasında kal
            return View("Index", dto);
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
