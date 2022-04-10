using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MiniBank.Core.Domains.Accounts.Repositories;
using MiniBank.Core.Domains.Users.UserMessages;
namespace MiniBank.Core.Domains.Accounts.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Account> _accountValidator;
        public AccountService(IValidator<Account> accountValidator, IUnitOfWork unitOfWork, IAccountRepository accountRepository)
        {
            _accountValidator = accountValidator;
            _unitOfWork = unitOfWork;
            _accountRepository = accountRepository;

        }
        public async Task<Account> Create(Account account)
        {
            _accountValidator.ValidateAndThrow(account);
            var _account = await _accountRepository.Create(account);
            await _unitOfWork.SaveChangesAsync();
            return _account;
        }
        public IEnumerable<Account> GetAll()
        {
            return _accountRepository.GetAll();
        }
        public async Task Delete(Guid Id)
        {
            await _accountRepository.Delete(Id);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}

