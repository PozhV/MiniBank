using MiniBank.Core.Domains.Users;
using MiniBank.Core.Domains.Users.Repositories;
using MiniBank.Core;
using Microsoft.EntityFrameworkCore;
namespace MiniBank.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MiniBankContext _context;

        public UserRepository(MiniBankContext context)
        {
            _context = context;
        }
        public async Task Edit(User user)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(it => it.UserId == user.UserId);
            if (entity == null)
            {
                throw new ValidationException("Невозможно редактировать логин и email. Пользователя с таким Id не существует");
            }
            entity.Login = user.Login;
            entity.Email = user.Email;
        }
        public async Task<User> Create(User user)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(it => it.Login == user.Login);
            if (entity != null)
            {
                throw new ValidationException("Невозможно добавить пользователя. Пользователь с таким логином уже существует");
            }
            Guid id = Guid.NewGuid();
            var userDb = new UserDbModel
            {
                UserId = Guid.NewGuid(),
                Login = user.Login,
                Email = user.Email
            };
            await _context.Users.AddAsync(userDb);
            user.UserId = id;
            return user;
        }
        public async Task Delete(Guid id)
        {
            if (! await IsUserExists(id))
            {
                throw new ValidationException("Пользователя с таким Id не существует");
            }
            var entity = await GetUserById(id);


            var  ent = await _context.Accounts.FirstOrDefaultAsync(it => it.UserId == id && it.IsOpen);
            if (ent!= null)
            {
                throw new ValidationException("Невозможно удалить пользователя. Есть незакрытые аккаунты");
            }
            _context.Users.Remove(entity);
        }
        public async Task<UserDbModel?> GetUserById(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(it => it.UserId == id);
        }
        public async Task<bool> IsUserExists(Guid id)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(it => it.UserId == id);
            if (entity == null)
            {
                return false;
            }
            return true;
        }
        public async Task<List<User>> GetAll()
        {
            return await _context.Users.Select(it => new User()
            {
                UserId = it.UserId,
                Login = it.Login,
                Email= it.Email
            }).ToListAsync();
         }
        public async Task<bool> ContainsByLogin(string login)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(it => it.Login == login);
            if (entity == null)
            {
                return false;
            }
            return true;
        }
    }
}
