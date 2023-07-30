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

        private readonly HttpClient _client;
        private readonly IUserMovieService _userMovieService;
        private readonly IWatchedMovieService _watchedMovieService;

        public MovieController(IUserMovieService userMovieService, IWatchedMovieService watchedMovieService)
        {
            _client = new HttpClient();
            _userMovieService = userMovieService;
            _watchedMovieService = watchedMovieService;
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
                return View();
            }

        }

        [HttpPost]
        public IActionResult Add(UserMovie userMovie)
        {
           
            var userId = HttpContext.Session.GetString("UserId");
            int.TryParse(userId, out var id);
            userMovie.UserId = id;

            var userMovies = _userMovieService.GetUserMovies(x => x.UserId == id);
            var existingImdbIds = userMovies.Select(m => m.ImdbId).ToList();

            
            if (!existingImdbIds.Contains(userMovie.ImdbId))
            {
                _userMovieService.Add(userMovie);
            }

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

        [HttpPost]
        public IActionResult AddWatchedMovie(WatchedMovie watchedMovie)
        {

            var userId = HttpContext.Session.GetString("UserId");
            int.TryParse(userId, out var id);
            watchedMovie.UserId = id;

            _watchedMovieService.Add(watchedMovie);

            return RedirectToAction("Index", "Movie");
        }
        public IActionResult PointWatchedMovie()
        {

            var userId = HttpContext.Session.GetString("UserId");
            int.TryParse(userId, out var id);
            var watchedMovies = _watchedMovieService.GetUserMovies(x => x.UserId == id);
            return View(watchedMovies);
        }
        [HttpPost]
        public IActionResult RemoveWatchedList(string imdbId)
        {
            var id = HttpContext.Session.GetString("UserId");
            int.TryParse(id, out var numberId);
            _watchedMovieService.DeleteMovie(imdbId, numberId);

            return RedirectToAction("Index", "Movie");
        }

    }
}

