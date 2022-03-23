using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
namespace MiniBank.Core.Domains.Users.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User Create(User user);
        void Delete(Guid id);
        void Edit(User user);
    }
}
