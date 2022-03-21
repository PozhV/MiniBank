using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Transactions.Services
{
    public interface ITransactionService
    {
        public void ExecuteTransaction(Transaction transaction);
        public decimal CalculateComission(Transaction transaction);
    }
}
