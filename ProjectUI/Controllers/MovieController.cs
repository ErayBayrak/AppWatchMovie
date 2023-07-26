using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectUI.Models;
using System.Net.Http.Headers;
namespace ProjectUI.Controllers
{
    public class MovieController : Controller
    {
        public async Task<IActionResult> Index(string title,string year)
        {
            List<ApiMovieViewModel> apiMovieViewModels = new List<ApiMovieViewModel>();
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://www.omdbapi.com/?apikey=469058fb&s=shrek&y=2010"),
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<OmdbApiResponseModel>(body);
                apiMovieViewModels = apiResponse.Search;
                return View(apiMovieViewModels.Take(1).ToList());

                //apiMovieViewModels = JsonConvert.DeserializeObject<List<ApiMovieViewModel>>(body);
                //return View(apiMovieViewModels[0]);
            }
        }
  
    }
}
