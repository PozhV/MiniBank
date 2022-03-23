using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniBank.Core.Domains.Users.UserMessages;
using MiniBank.Core.Domains.Accounts.Repositories;
using MiniBank.Core;
namespace MiniBank.Core.Domains.Transactions.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRatesDatabase _ratesDatabase;
        private readonly IConverter _converter;
        public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository, IRatesDatabase ratesDatabase, IConverter converter)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _ratesDatabase = ratesDatabase;
            _converter = converter;
        }
        public void ExecuteTransaction(Transaction transaction)
        {
            if (transaction.FromAccountId == transaction.ToAccountId)
            {
                throw new ValidationException("Нельзя перевести деньги на тот же аккаунт");
            }
            string fromAccountCurrencyName = _accountRepository.GetCurrencyName(transaction.FromAccountId);
            string toAccountCurrencyName = _accountRepository.GetCurrencyName( transaction.ToAccountId);
            decimal comissionPercent = _transactionRepository.CalculateComissionPercent(transaction.FromAccountId, transaction.ToAccountId);
            decimal rate = _ratesDatabase.GetRate(fromAccountCurrencyName, toAccountCurrencyName);
            decimal toAccountAmount = Decimal.Round(_converter.Convert(transaction.Amount, rate) * (1 - comissionPercent), 2);

            _transactionRepository.ExecuteTransaction(transaction.Amount, toAccountAmount, transaction.FromAccountId, transaction.ToAccountId);
        }
        public decimal CalculateComission(Transaction transaction)
        {
            if (transaction.Amount <= 0)
            {
                throw new ValidationException("Cумма перевода должна быть больше нуля", new TransactionExceptionMessage()
                {
                    WrongAmount = transaction.Amount
                });
            }
            if (transaction.FromAccountId == Guid.Empty)
            {
                throw new ValidationException("Не задан id аккаунта, с которого будет совершен перевод");
            }
            if (transaction.ToAccountId == Guid.Empty)
            {
                throw new ValidationException("Не задан id аккаунта, на который будет совершен перевод");
            }
            decimal comissionPercent = _transactionRepository.CalculateComissionPercent(transaction.FromAccountId, transaction.ToAccountId);
            return Decimal.Round(comissionPercent * transaction.Amount, 2);
        }
    }
}
