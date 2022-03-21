using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniBank.Data.Accounts.Repositories;
using MiniBank.Core;
using MiniBank.Core.Domains.Transactions;
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
        public void ExecuteTransaction(decimal amount, string fromAccountId, string toAccountId)
        { 
            decimal comission = CalculateComission(amount, fromAccountId, toAccountId);
            if (_accounts[fromAccountId].Balance < amount)
            {
                throw new ValidationException("На счете недостаточно средств для перевода");
            }
            decimal rate = _rateDatabase.GetRate(_accounts[fromAccountId].CurrencyName, _accounts[toAccountId].CurrencyName);
            decimal toAccountAmount = _converter.Convert(amount, rate);
            _accounts[toAccountId].Balance += toAccountAmount - comission;
            _accounts[fromAccountId].Balance -= amount;
            _transactionStorage.Add(new TransactionDbModel
            {
                TransactionId = Guid.NewGuid().ToString(),
                Amount = toAccountAmount,
                CurrencyName = _accounts[toAccountId].CurrencyName,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId
            });
        }
        public decimal CalculateComission(decimal amount, string fromAccountId, string toAccountId)
        {
            if (!_accounts.ContainsKey(fromAccountId) || !_accounts.ContainsKey(toAccountId))
            {
                throw new ValidationException("Одного из аккаунтов не существует");
            }
            if (!_accounts[fromAccountId].IsOpen || !_accounts[toAccountId].IsOpen)
            {
                throw new ValidationException("Один из аккунтов закрыт");
            }
            if (_accounts[fromAccountId].UserId == _accounts[toAccountId].UserId)
            {
                return 0;
            }
            decimal rate = _rateDatabase.GetRate(_accounts[fromAccountId].CurrencyName, _accounts[toAccountId].CurrencyName);
            return Decimal.Round(Convert.ToDecimal(0.02) * _converter.Convert(amount, rate), 2);
        }
    }
}
