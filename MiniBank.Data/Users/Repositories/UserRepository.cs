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
            var entity = await _context.Users.FirstOrDefaultAsync(it => it.UserId == id);

            if (entity == null)
            {
                throw new ValidationException("Пользователя с таким Id не существует");
            }
            var  ent = _context.Accounts.FirstOrDefault(it => it.UserId == id && it.IsOpen);
            if (ent!= null)
            {
                throw new ValidationException("Невозможно удалить пользователя. Есть незакрытые аккаунты");
            }
            _context.Users.Remove(entity);
        }
        public Task<bool> IsUserExists(Guid id)
        {
            var entity = _context.Users.FirstOrDefault(it => it.UserId == id);
            if (entity == null)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
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
    }
}
