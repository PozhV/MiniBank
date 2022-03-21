using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniBank.Core.Domains.Users.UserMessages;
namespace MiniBank.Core.Domains.Transactions.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public void ExecuteTransaction(Transaction transaction)
        {
            if(transaction.Amount <= 0)
            {
                throw new ValidationException("Cумма перевода должна быть больше нуля", new TransactionExceptionMessage() 
                {
                    WrongAmount = transaction.Amount
                });
            }
            if(String.IsNullOrEmpty(transaction.FromAccountId))
            {
                throw new ValidationException("Не задан id аккаунта, с которого будет совершен перевод");
            }
            if (String.IsNullOrEmpty(transaction.ToAccountId))
            {
                throw new ValidationException("Не задан id аккаунта, на который будет совершен перевод");
            }
            if (transaction.FromAccountId == transaction.ToAccountId)
                return;
            _transactionRepository.ExecuteTransaction(transaction.Amount, transaction.FromAccountId, transaction.ToAccountId);
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
            if (String.IsNullOrEmpty(transaction.FromAccountId))
            {
                throw new ValidationException("Не задан id аккаунта, с которого будет совершен перевод");
            }
            if (String.IsNullOrEmpty(transaction.ToAccountId))
            {
                throw new ValidationException("Не задан id аккаунта, на который будет совершен перевод");
            }
            return _transactionRepository.CalculateComission(transaction.Amount, transaction.FromAccountId, transaction.ToAccountId);
        }
    }
}
