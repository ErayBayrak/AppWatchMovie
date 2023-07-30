using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectUI.Models;
using System.Net.Http.Headers;
using System.Security.Claims;

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
        private readonly IUserMovieService _userMovieService;

        public MovieController(IUserMovieService userMovieService)
        {
            _client = new HttpClient();
            _userMovieService = userMovieService;
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

        [HttpPost]
        public IActionResult Add(UserMovie userMovie)
        {
           
            var userId = HttpContext.Session.GetString("UserId");
            int.TryParse(userId, out var id);
            userMovie.UserId = id;
            
            _userMovieService.Add(userMovie);
 
            return RedirectToAction("Index","Movie");
        }
      public IActionResult WatchedMovie()
        {
            
            var userId = HttpContext.Session.GetString("UserId");
            int.TryParse(userId, out var id);
            var watchedMovies = _userMovieService.GetUserMovies(x=>x.UserId==id);
            return View(watchedMovies);
        }
        [HttpPost]
        public IActionResult Remove(string imdbId)
        {
            var id = HttpContext.Session.GetString("UserId");
            int.TryParse(id, out var numberId);
            _userMovieService.DeleteMovie(imdbId,numberId);

            return RedirectToAction("Index", "Movie");
        }

    }
}

