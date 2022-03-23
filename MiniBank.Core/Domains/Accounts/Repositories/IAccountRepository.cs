using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Accounts.Repositories
{
    public interface IAccountRepository
    {
        Account Create(Account account);
        void Delete(Guid Id);
        IEnumerable<Account> GetAll();
        string GetCurrencyName(Guid Id);
    }
}
