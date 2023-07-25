using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService
    {
        void Add(User user);
        void Delete(User user);
        void Update(User user);
        User Get(Expression<Func<User, bool>> filter);
        List<User> GetUsers(Expression<Func<User, bool>> filter = null);
    }
}
