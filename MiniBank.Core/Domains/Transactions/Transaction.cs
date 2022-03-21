using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Transactions
{
    public class Transaction
    {
        public decimal Amount { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
