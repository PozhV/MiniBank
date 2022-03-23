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
        public static Dictionary<Guid, AccountDbModel> _accounts = new Dictionary<Guid, AccountDbModel>();
        private readonly IRatesDatabase _ratesDatabase;
        public AccountRepository(IRatesDatabase ratesDatabase)
        {
            _ratesDatabase = ratesDatabase;
        }
        public string GetCurrencyName(Guid AccountId)
        {
            if (!_accounts.ContainsKey(AccountId))
            {
                throw new ValidationException("Одного из аккаунтов не существует");
            }
            return _accounts[AccountId].CurrencyName;
        }
        public Account Create(Account account)
        {
            if (!_userStorage.ContainsKey(account.UserId))
            {
                throw new ValidationException("Пользователя с таким Id не существует");
            }
            _ratesDatabase.CheckCurrencyName(account.CurrencyName);
            Guid id = Guid.NewGuid();
            _accounts[id] = new AccountDbModel
            {
                Id = id,
                UserId = account.UserId,
                CurrencyName = account.CurrencyName,
                Balance = account.Balance,
                IsOpen = true,
                OpenDate = DateTime.UtcNow
            };
            account.Id = id;
            account.IsOpen = true;
            return account;
        }
        public IEnumerable<Account> GetAll()
        {
            return _accounts.Select(it => new Account
            {
                Id = it.Value.Id,
                UserId = it.Value.UserId,
                Balance = it.Value.Balance,
                CurrencyName=it.Value.CurrencyName,
                IsOpen = it.Value.IsOpen
            });
        }
        public void Delete(Guid id)
        {
            if(!_accounts.ContainsKey(id))
            {
                throw new ValidationException("Аккаунта с таким Id не существует");  
            }
            if(_accounts[id].Balance > 0)
            {
                throw new ValidationException("Невозможно удалить аккаунт. Сумма на счете больше нуля", new AccountExceptionMessage()
                {
                    WrongBalance = _accounts[id].Balance
                });
            }
            _accounts[id].IsOpen = false;
            _accounts[id].CloseDate = DateTime.UtcNow;
        }
        
    }
}
