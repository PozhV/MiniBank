using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Transactions
{
    public interface ITransactionRepository
    {
        public void ExecuteTransaction(decimal fromAccountAmount, decimal toAccountAmount, Guid fromAccountId, Guid toAccountId);
        public decimal CalculateComissionPercent(Guid fromAccountId, Guid toAccountId);
    }
}
