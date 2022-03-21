using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
namespace MiniBank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User Create(User user);
        void Delete(string id);
        void Edit(User user);
    }

}
