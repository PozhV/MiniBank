using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Transactions
{
    public interface ITransactionRepository
    {
        public void ExecuteTransaction(decimal Amount, string FromAccountId, string ToAccountId);
        public decimal CalculateComission(decimal Amount, string FromAccountId, string ToAccountId);
    }
}
