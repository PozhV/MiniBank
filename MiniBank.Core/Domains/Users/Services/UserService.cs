using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MiniBank.Core.Domains.Users.Repositories;
namespace MiniBank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User Create(User user)
        {
            if (String.IsNullOrEmpty(user.Login))
            {
                throw new ValidationException("Не задан логин");
            }
            if(String.IsNullOrEmpty(user.Email))
            {
                throw new ValidationException("Не задан email");
            }
            return _userRepository.Create(user);
        }
        public void Edit(User user)
        {
            if(String.IsNullOrEmpty(user.UserId))
            {
                throw new ValidationException("Не задан Id");
            }
            if (String.IsNullOrEmpty(user.Login))
            {
                throw new ValidationException("Не задан логин");
            }
            if (String.IsNullOrEmpty(user.Email))
            {
                throw new ValidationException("Не задан email");
            }
            _userRepository.Edit(user);
        }
        public void Delete(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new ValidationException("Не задан Id пользователя");
            }
            _userRepository.Delete(id);
        }
    }
}
