using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Transaction> _transactionValidator;
        public TransactionService(IValidator<Transaction> transactionValidator, IUnitOfWork unitOfWork, ITransactionRepository transactionRepository, 
            IAccountRepository accountRepository, IRatesDatabase ratesDatabase, IConverter converter)
        {
            _transactionValidator = transactionValidator;
            _unitOfWork = unitOfWork;
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _ratesDatabase = ratesDatabase;
            _converter = converter;
        }
        public async Task ExecuteTransaction(Transaction transaction)
        {
            _transactionValidator.ValidateAndThrow(transaction);
            decimal comissionPercent = await _transactionRepository.CalculateComissionPercent(
                transaction.FromAccountId, transaction.ToAccountId);
            string fromAccountCurrencyName = await _accountRepository.GetCurrencyName(transaction.FromAccountId);
            string toAccountCurrencyName = await _accountRepository.GetCurrencyName(transaction.ToAccountId);
            decimal rate = await _ratesDatabase.GetRate(
                fromAccountCurrencyName, toAccountCurrencyName);
            decimal toAccountAmount = Decimal.Round(
                _converter.Convert(transaction.Amount, rate) * (1 - comissionPercent), 2);

            await _transactionRepository.ExecuteTransaction(transaction.Amount, toAccountAmount,
                transaction.FromAccountId, transaction.ToAccountId);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<decimal> CalculateComission(Transaction transaction)
        {
            _transactionValidator.ValidateAndThrow(transaction);
            decimal comissionPercent = await _transactionRepository.CalculateComissionPercent(transaction.FromAccountId, transaction.ToAccountId);
            return Decimal.Round(comissionPercent * transaction.Amount, 2);
        }
    }
}
