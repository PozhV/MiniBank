﻿using System;
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
        IEnumerable<Account> GetAll();
        Task<string> GetCurrencyName(Guid Id);
        Task<bool> IsAccountExists(Guid Id);
        Task<bool> IsAccountOpen (Guid Id);
    }
}
