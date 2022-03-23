using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniBank.Core.Domains.Accounts.Services;
using MiniBank.Web.Controllers.Accounts.Dto;
using MiniBank.Web.Controllers.Transactions.Dto;
using MiniBank.Core.Domains.Transactions;
using MiniBank.Core.Domains.Transactions.Services;
using MiniBank.Core.Domains.Accounts;
namespace MiniBank.Web.Controllers.Accounts
{
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
        public Account CreateAccount(AccountDto model)
        {
            return _accountService.Create(new Account
            {
                UserId = model.UserId,
                CurrencyName = model.CurrencyName,
                Balance = model.Balance
            });
        }
        [HttpGet]
        public IEnumerable<Account> List()
        {
            return _accountService.GetAll();
        }
        [HttpPut]
        public void ExecuteTransaction([FromQuery]TransactionDto model)
        {
            _transactionService.ExecuteTransaction(new Transaction
            {
                Amount = model.Amount,
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId
            });
        }
        [HttpGet]
        public decimal TransactionComission([FromQuery]TransactionDto model)
        {
            return _transactionService.CalculateComission(new Transaction
            {
                Amount = model.Amount,
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId
            });
        }
        [HttpDelete]
        public void DeleteAccount(Guid id)
        {
            _accountService.Delete(id);
        }
    }
}