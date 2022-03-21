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
        public static Dictionary<string, AccountDbModel> _accounts = new Dictionary<string, AccountDbModel>();
        private readonly IRatesDatabase _ratesDatabase;
        public AccountRepository(IRatesDatabase ratesDatabase)
        {
            _ratesDatabase = ratesDatabase;
        }
        public Account Create(Account account)
        {
            if (!_userStorage.ContainsKey(account.UserId))
            {
                throw new ValidationException("Пользователя с таким Id не существует");
            }
            _ratesDatabase.CheckCurrencyName(account.CurrencyName);
            string id = Guid.NewGuid().ToString();
            _accounts[id] = new AccountDbModel
            {
                Id = id,
                UserId = account.UserId,
                CurrencyName = account.CurrencyName,
                Balance = account.Balance,
                IsOpen = true,
                OpenDate = DateTime.Now
            };
            account.Id = id;
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
        public void Delete(string id)
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
            _accounts[id].CloseDate = DateTime.Now;
        }
        
    }
}
