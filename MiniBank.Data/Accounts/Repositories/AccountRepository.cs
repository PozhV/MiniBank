using Microsoft.EntityFrameworkCore;
using MiniBank.Core.Domains.Accounts.Repositories;
using MiniBank.Core;
using MiniBank.Core.Domains.Accounts;
using MiniBank.Data.HttpClients;
using static MiniBank.Data.Users.Repositories.UserRepository;
using MiniBank.Core.Domains.Users.UserMessages;
namespace MiniBank.Data.Accounts.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IRatesDatabase _ratesDatabase;
        private readonly MiniBankContext _context;
        public AccountRepository(MiniBankContext context, IRatesDatabase ratesDatabase)
        {
            _ratesDatabase = ratesDatabase;
            _context = context;
        }
        public async Task<string> GetCurrencyName(Guid AccountId)
        {
            var entity = await _context.Accounts.FirstOrDefaultAsync(it => it.Id == AccountId);
            return entity.CurrencyName;
        }
        public async Task<Account> Create(Account account)
        {
            var entity = await _context.Accounts.FirstOrDefaultAsync(it => it.UserId == account.UserId);
            _ratesDatabase.CheckCurrencyName(account.CurrencyName);
            Guid id = Guid.NewGuid();
            await _context.Accounts.AddAsync(new AccountDbModel
            {
                Id = id,
                UserId = account.UserId,
                CurrencyName = account.CurrencyName,
                Balance = account.Balance,
                IsOpen = true,
                OpenDate = DateTime.UtcNow
            });
            account.Id = id;
            account.IsOpen = true;
            return account;
        }
        
        public async Task Delete(Guid id)
        {
            var entity = await _context.Accounts.FirstOrDefaultAsync(it => it.Id == id);
            if (entity == null)
            {
                throw new ValidationException("Аккаунта с таким Id не существует");  
            }
            if(!entity.IsOpen)
            {
                throw new ValidationException("Аккаунт с таким Id уже закрыт");
            }
            if(entity.Balance > 0)
            {
                throw new ValidationException("Невозможно удалить аккаунт. Сумма на счете больше нуля", new AccountExceptionMessage()
                {
                    WrongBalance = entity.Balance
                });
            }
            entity.IsOpen = false;
            entity.CloseDate = DateTime.UtcNow;
        }
        public async Task<bool> IsAccountExists(Guid id)
        {
            var entity = await _context.Accounts.FirstOrDefaultAsync(it => it.Id == id);
            if (entity == null)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> IsAccountOpen(Guid id)
        {
            var entity = await _context.Accounts.FirstOrDefaultAsync(it => it.Id == id && it.IsOpen);
            if (entity == null)
            {
                return false;
            }
            return true;
        }
        public IEnumerable<Account> GetAll()
        {
            return _context.Accounts.Select(it => new Account
            {
                Id = it.Id,
                UserId = it.UserId,
                Balance = it.Balance,
                CurrencyName = it.CurrencyName,
                IsOpen = it.IsOpen
            });
        }

    }
}
