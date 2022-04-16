using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Core.Domains.Accounts.Services
{
    public interface IAccountService
    {
        Task<Account> Create(Account account);
        Task Delete(Guid UserId);
        Task<List<Account>> GetAll();
    }
}
