using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class WatchedMovieManager : IWatchedMovieService
    {
        IWatchedMovieDal _watchedMovieDal;
        public WatchedMovieManager(IWatchedMovieDal watchedMovieDal)
        {
            _watchedMovieDal = watchedMovieDal;
        }
        public void Add(WatchedMovie watchedMovie)
        {
            _watchedMovieDal.Add(watchedMovie);
        }

        public void Delete(WatchedMovie watchedMovie)
        {
            _watchedMovieDal.Delete(watchedMovie);
        }

        public WatchedMovie Get(Expression<Func<WatchedMovie, bool>> filter)
        {
            return _watchedMovieDal.Get(filter);
        }

        public List<WatchedMovie> GetUserMovies(Expression<Func<WatchedMovie, bool>> filter = null)
        {
            return _watchedMovieDal.GetAll(filter);
        }

        public void Update(WatchedMovie watchedMovie)
        {
            _watchedMovieDal.Update(watchedMovie);
        }
        public void DeleteMovie(string imdbId, int userId)
        {
            var removedMovie = _watchedMovieDal.Get(x => x.ImdbId == imdbId && x.UserId == userId);
            _watchedMovieDal.Delete(removedMovie);
        }
    }
}
