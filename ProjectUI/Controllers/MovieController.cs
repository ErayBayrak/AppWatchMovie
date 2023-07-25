using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectUI.Models;
using System.Net.Http.Headers;
namespace ProjectUI.Controllers
{
    public class MovieController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<ApiMovieViewModel> apiMovieViewModels = new List<ApiMovieViewModel>();
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://imdb-top-100-movies.p.rapidapi.com/"),
                Headers =
    {
        { "X-RapidAPI-Key", "8aeece6e7emshddc8f9b9159764dp18957fjsne809f1fcc81d" },
        { "X-RapidAPI-Host", "imdb-top-100-movies.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                apiMovieViewModels = JsonConvert.DeserializeObject<List<ApiMovieViewModel>>(body);
                return View(apiMovieViewModels);
            }
        }
        public async Task<IActionResult> GetMovie(string topNumber)
        {
            ApiMovieViewModel model = new ApiMovieViewModel();
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://imdb-top-100-movies.p.rapidapi.com/top1"),
                Headers =
    {
        { "X-RapidAPI-Key", "8aeece6e7emshddc8f9b9159764dp18957fjsne809f1fcc81d" },
        { "X-RapidAPI-Host", "imdb-top-100-movies.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<ApiMovieViewModel>(body);
                return View(model);
            }

        }
    }
}
