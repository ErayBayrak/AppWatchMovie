using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserMovieManager : IUserMovieService
    {
        IUserMovieDal _userMovieDal;
        public UserMovieManager(IUserMovieDal userMovieDal)
        {
            _userMovieDal = userMovieDal;
        }
        public void Add(UserMovie userMovie)
        {
            _userMovieDal.Add(userMovie);
        }
        
        public void Delete(UserMovie userMovie)
        {
            _userMovieDal.Delete(userMovie);
        }

        public UserMovie Get(Expression<Func<UserMovie, bool>> filter)
        {
            return _userMovieDal.Get(filter);
        }

        public List<UserMovie> GetUserMovies(Expression<Func<UserMovie, bool>> filter = null)
        {
            return _userMovieDal.GetAll(filter);
        }

        public void Update(UserMovie userMovie)
        {
            _userMovieDal.Update(userMovie);
        }
    }
}
