using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Accounts.Services
{
    public interface IAccountService
    {
        Account Create(Account account);
        void Delete(Guid UserId);
        IEnumerable<Account> GetAll();
    }
}
