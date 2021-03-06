using Microsoft.AspNetCore.Mvc;
using MiniBank.Core.Domains.Accounts.Services;
using MiniBank.Web.Controllers.Accounts.Dto;
using MiniBank.Web.Controllers.Transactions.Dto;
using MiniBank.Core.Domains.Transactions;
using MiniBank.Core.Domains.Transactions.Services;
using MiniBank.Core.Domains.Accounts;
using Microsoft.AspNetCore.Authorization;

namespace MiniBank.Web.Controllers.Accounts
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]", Name = "[controller]_[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        public AccountController(IAccountService accountService, ITransactionService transactionService)
        {
            _accountService = accountService;
            _transactionService = transactionService;
        }

        [HttpPost]
        public async Task<Account> CreateAccount(AccountDto model)
        {
            return await _accountService.Create(new Account
            {
                UserId = model.UserId,
                CurrencyName = model.CurrencyName,
                Balance = model.Balance
            });
        }
        [HttpGet]
        public async Task<List<Account>> List()
        {
            return await _accountService.GetAll();
        }
        [HttpPut]
        public async Task ExecuteTransaction([FromQuery]TransactionDto model)
        {
            await _transactionService.ExecuteTransaction(new Transaction
            {
                Amount = model.Amount,
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId
            });
        }
        [HttpGet]
        public async Task<decimal> TransactionComission([FromQuery]TransactionDto model)
        {
            return await _transactionService.CalculateComission(new Transaction
            {
                Amount = model.Amount,
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId
            });
        }
        [HttpDelete]
        public async Task DeleteAccount(Guid id)
        {
            await _accountService.Delete(id);
        }
    }
}