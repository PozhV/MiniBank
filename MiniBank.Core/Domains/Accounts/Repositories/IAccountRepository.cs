using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Accounts.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> Create(Account account);
        Task Delete(Guid Id);
        Task<List<Account>> GetAll();
        Task<string> GetCurrencyName(Guid Id);
    }
}
