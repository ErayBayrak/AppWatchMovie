using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IWatchedMovieService
    {
        void Add(WatchedMovie watchedMovie);
        void Delete(WatchedMovie watchedMovie);
        void Update(WatchedMovie watchedMovie);
        WatchedMovie Get(Expression<Func<WatchedMovie, bool>> filter);
        List<WatchedMovie> GetUserMovies(Expression<Func<WatchedMovie, bool>> filter = null);
        void DeleteMovie(string imdbId, int userId);
    }
}
