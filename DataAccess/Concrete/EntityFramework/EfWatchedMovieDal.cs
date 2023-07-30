using DataAccess.Abstract;
using DataAccess.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfWatchedMovieDal : EfEntityRepositoryBase<WatchedMovie, Context>, IWatchedMovieDal
    {
    }
}
