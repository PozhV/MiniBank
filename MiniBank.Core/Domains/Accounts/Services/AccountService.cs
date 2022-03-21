using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniBank.Core.Domains.Accounts.Repositories;
using MiniBank.Core.Domains.Users.UserMessages;
namespace MiniBank.Core.Domains.Accounts.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public Account Create(Account account)
        {
            if (account.Balance < 0)
            {
                throw new ValidationException("Начальный баланс должен быть не меньше нуля", new AccountExceptionMessage(){WrongBalance = account.Balance});
            }
            if (String.IsNullOrEmpty(account.CurrencyName))
            {
                throw new ValidationException("Не задано название валюты");
            }
            if (String.IsNullOrEmpty(account.UserId))
            {
                throw new ValidationException("Не задан id пользователя, к которому будет привязан аккаунт");
            }
            return _accountRepository.Create(account);
        }
        public void Delete(string Id)
        {
            if (String.IsNullOrEmpty(Id))
                throw new ValidationException("Id аккаунта не задан");
            _accountRepository.Delete(Id);
        }

    }
}

