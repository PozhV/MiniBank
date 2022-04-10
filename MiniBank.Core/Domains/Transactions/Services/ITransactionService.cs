using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Transactions.Services
{
    public interface ITransactionService
    {
        public Task ExecuteTransaction(Transaction transaction);
        public Task<decimal> CalculateComission(Transaction transaction);
    }
}
