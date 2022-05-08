using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Users.UserMessages
{
    public class AccountExceptionMessage
    {
        public decimal WrongBalance { get; set; }
    }
}
