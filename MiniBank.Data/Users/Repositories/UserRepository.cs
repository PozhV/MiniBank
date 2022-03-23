using MiniBank.Core.Domains.Users;
using MiniBank.Core.Domains.Users.Repositories;
using MiniBank.Core;
using static MiniBank.Data.Accounts.Repositories.AccountRepository;
namespace MiniBank.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        public static Dictionary<Guid, UserDbModel> _userStorage = new Dictionary<Guid, UserDbModel>();
        public void Edit(User user)
        {
            if(!_userStorage.ContainsKey(user.UserId))
            {
                throw new ValidationException("Невозможно редактировать логин и email. Пользователя с таким Id не существует");
            }
            _userStorage[user.UserId] = new UserDbModel
            {
                UserId = user.UserId,
                Login = user.Login,
                Email = user.Email
            };
        }
        public User Create(User user)
        {
            var entity = _userStorage.FirstOrDefault(it => it.Value.Login == user.Login);
            if (entity.Value != null)
            {
                throw new ValidationException("Невозможно добавить пользователя. Пользователь с таким логином уже существует");
            }
            Guid id = Guid.NewGuid();
            _userStorage[id]= new UserDbModel
            {
                UserId = id,
                Login = user.Login,
                Email = user.Email
            };
            user.UserId = id;
            return user;
        }
        public IEnumerable<User> GetAll()
        {
            return _userStorage.Select(it => new User
            {
                UserId = it.Value.UserId,
                Login = it.Value.Login,
                Email = it.Value.Email
            });
        }

        public void Delete(Guid id)
        {
            if (!_userStorage.ContainsKey(id))
            {
                throw new ValidationException("Пользователя с таким Id не существует");
            }
            var  entity = _accounts.FirstOrDefault(it => it.Value.UserId == id && it.Value.IsOpen);
            if (entity.Value != null)
            {
                throw new ValidationException("Невозможно удалить пользователя. Есть незакрытые аккаунты");
            }
            
            _userStorage.Remove(id);
        }
    }
}
