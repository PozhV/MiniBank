using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniBank.Data.Accounts.Repositories;
using MiniBank.Core;
using MiniBank.Core.Domains.Transactions;
using MiniBank.Core.Domains.Users.UserMessages;
using static MiniBank.Data.Accounts.Repositories.AccountRepository;
namespace MiniBank.Data.Transactions.Repositories
{

    public class TransactionRepository : ITransactionRepository
    {
        private readonly MiniBankContext _context;
        public TransactionRepository(MiniBankContext context, IConverter converter, IRatesDatabase ratesDatabase)
        {
            _context = context;
        }
        public async Task ExecuteTransaction(decimal fromAccountAmount, decimal toAccountAmount, Guid fromAccountId, Guid toAccountId)
        {
            var entityFrom = await _context.Accounts.FirstOrDefaultAsync(it => it.Id == fromAccountId);
            var entityTo = await _context.Accounts.FirstOrDefaultAsync(it => it.Id == toAccountId);
            if (entityFrom.Balance < fromAccountAmount)
            {
                throw new ValidationException("На счете недостаточно средств для перевода", new AccountExceptionMessage
                {
                    WrongBalance = entityFrom.Balance - fromAccountAmount,
                });
            }

            entityTo.Balance += toAccountAmount;
            entityFrom.Balance -= fromAccountAmount;
            await _context.Transactions.AddAsync(new TransactionDbModel
            {
                TransactionId = Guid.NewGuid(),
                Amount = toAccountAmount,
                CurrencyName = entityTo.CurrencyName,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId
            });
        }
        public async Task<decimal> CalculateComissionPercent(Guid fromAccountId, Guid toAccountId)
        {
            var entityFrom = await _context.Accounts.FirstOrDefaultAsync(it => it.Id == fromAccountId);
            var entityTo = await _context.Accounts.FirstOrDefaultAsync(it => it.Id == toAccountId);
            if (entityFrom == null || entityTo == null)
            {
                throw new ValidationException("Одного из аккунтов не существует");
            }
            if (! entityTo.IsOpen || !entityTo.IsOpen)
            {
                throw new ValidationException("Один из аккунтов закрыт");
            }

            if (entityFrom.UserId == entityTo.UserId)
            {
                return 0;
            }
            return Convert.ToDecimal(0.02);
        }
    }
}
