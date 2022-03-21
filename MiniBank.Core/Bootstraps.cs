using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniBank.Core.Domains.Users.Repositories;
using MiniBank.Core.Domains.Users.Services;
using MiniBank.Core.Domains.Accounts.Services;
using MiniBank.Core.Domains.Transactions.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions;
namespace MiniBank.Core
{
    public static class Bootstraps
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IConverter, Converter>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITransactionService, TransactionService>();
            return services;
        }
    }

}
