using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Users.UserMessages
{
    public class TransactionExceptionMessage
    {
        public decimal WrongAmount { get; set; }
    }
}
