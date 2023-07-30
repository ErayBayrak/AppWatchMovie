using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserMovieService
    {
        void Add(UserMovie userMovie);
        void Delete(UserMovie userMovie);
        void Update(UserMovie userMovie);
        UserMovie Get(Expression<Func<UserMovie, bool>> filter);
        List<UserMovie> GetUserMovies(Expression<Func<UserMovie, bool>> filter = null);
    }
}
