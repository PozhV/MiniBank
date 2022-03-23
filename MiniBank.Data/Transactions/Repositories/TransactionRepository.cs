using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniBank.Data.Accounts.Repositories;
using MiniBank.Core;
using MiniBank.Core.Domains.Transactions;
using MiniBank.Core.Domains.Users.UserMessages;
using static MiniBank.Data.Accounts.Repositories.AccountRepository;
namespace MiniBank.Data.Transactions.Repositories
{

    public class TransactionRepository : ITransactionRepository
    {
        private static List<TransactionDbModel> _transactionStorage = new List<TransactionDbModel>();
        private readonly IConverter _converter;
        private readonly IRatesDatabase _rateDatabase;
        public TransactionRepository(IConverter converter, IRatesDatabase ratesDatabase)
        {
            _converter = converter;
            _rateDatabase = ratesDatabase;
        }
        public void ExecuteTransaction(decimal fromAccountAmount, decimal toAccountAmount, Guid fromAccountId, Guid toAccountId)
        { 
            if (_accounts[fromAccountId].Balance < fromAccountAmount)
            {
                throw new ValidationException("На счете недостаточно средств для перевода", new AccountExceptionMessage
                {
                    WrongBalance = _accounts[fromAccountId].Balance - fromAccountAmount,
                });
            }

            _accounts[toAccountId].Balance += toAccountAmount;
            _accounts[fromAccountId].Balance -= fromAccountAmount;
            _transactionStorage.Add(new TransactionDbModel
            {
                TransactionId = Guid.NewGuid(),
                Amount = toAccountAmount,
                CurrencyName = _accounts[toAccountId].CurrencyName,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId
            });
        }
        public decimal CalculateComissionPercent(Guid fromAccountId, Guid toAccountId)
        {
            if(!_accounts.ContainsKey(fromAccountId) || !_accounts.ContainsKey(toAccountId))
                throw new ValidationException("Одного из аккунтов не существует");
            if (!_accounts[fromAccountId].IsOpen || !_accounts[toAccountId].IsOpen)
            {
                throw new ValidationException("Один из аккунтов закрыт");
            }
            if (_accounts[fromAccountId].UserId == _accounts[toAccountId].UserId)
            {
                return 0;
            }
            return Convert.ToDecimal(0.02);
        }
    }
}
