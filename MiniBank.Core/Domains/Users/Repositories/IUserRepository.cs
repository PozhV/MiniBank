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
        Task<List<User>> GetAll();
        Task<User> Create(User user);
        Task Delete(Guid id);
        Task Edit(User user);
        Task<bool> IsUserExists(Guid id);
        Task<bool> ContainsByLogin(string login);
    }
}
