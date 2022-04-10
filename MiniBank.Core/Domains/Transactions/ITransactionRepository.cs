using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Transactions
{
    public interface ITransactionRepository
    {
        public Task ExecuteTransaction(decimal fromAccountAmount, decimal toAccountAmount, Guid fromAccountId, Guid toAccountId);
        public Task<decimal> CalculateComissionPercent(Guid fromAccountId, Guid toAccountId);
    }
}
