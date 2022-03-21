using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Data.Transactions
{
    public class TransactionDbModel
    {
        public string TransactionId { get; set; }
        public string CurrencyName { get; set; }
        public decimal Amount { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
