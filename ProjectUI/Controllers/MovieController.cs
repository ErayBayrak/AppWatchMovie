using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectUI.Models;
using System.Net.Http.Headers;
namespace ProjectUI.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        //public async Task<IActionResult> Index(string title,string year)
        //{
        //    List<ApiMovieViewModel> apiMovieViewModels = new List<ApiMovieViewModel>();
        //    var client = new HttpClient();
        //    var request = new HttpRequestMessage
        //    {
        //        Method = HttpMethod.Get,
        //        RequestUri = new Uri($"http://www.omdbapi.com/?apikey=469058fb&s=shrek&y=2010"),
        //    };
        //    using (var response = await client.SendAsync(request))
        //    {
        //        response.EnsureSuccessStatusCode();
        //        var body = await response.Content.ReadAsStringAsync();
        //        var apiResponse = JsonConvert.DeserializeObject<OmdbApiResponseModel>(body);
        //        apiMovieViewModels = apiResponse.Search;
        //        return View(apiMovieViewModels.Take(1).ToList());

        //        //apiMovieViewModels = JsonConvert.DeserializeObject<List<ApiMovieViewModel>>(body);
        //        //return View(apiMovieViewModels[0]);
        //    }
        //}

        private readonly HttpClient _client;

        public MovieController()
        {
            _client = new HttpClient();
        }

        public async Task<IActionResult> Index(string title, string year)
        {

            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(year))
            {
                List<ApiMovieViewModel> apiMovieViewModels = new List<ApiMovieViewModel>();
                var requestUri = $"http://www.omdbapi.com/?apikey=469058fb&s={title}&y={year}";

                using (var response = await _client.GetAsync(requestUri))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<OmdbApiResponseModel>(body);
                    apiMovieViewModels = apiResponse.Search;
                }

                return View(apiMovieViewModels);
            }
            else
            {
                List<ApiMovieViewModel> apiMovieViewModels = new List<ApiMovieViewModel>();
                var requestUri = $"http://www.omdbapi.com/?apikey=469058fb&s=shrek&y=2010";

                using (var response = await _client.GetAsync(requestUri))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<OmdbApiResponseModel>(body);
                    apiMovieViewModels = apiResponse.Search;
                }

                return View(apiMovieViewModels);
            }
           
        }

    }
}
