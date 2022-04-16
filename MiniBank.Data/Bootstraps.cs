using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MiniBank.Data.Accounts.Repositories;
using MiniBank.Data.Users.Repositories;
using MiniBank.Core.Domains.Accounts.Repositories;
using MiniBank.Core.Domains.Transactions;
using MiniBank.Data.Transactions.Repositories;
using MiniBank.Core.Domains.Users.Repositories;
using MiniBank.Core;
namespace MiniBank.Data
{
    public static class Bootstraps
    {
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IRatesDatabase, RatesDatabase>(options =>
            {
                options.BaseAddress = new Uri(configuration["CentralBankUri"]);

            });
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<MiniBankContext>(options => options.UseNpgsql(configuration["DbConnectionString"]));

            return services;
        }
    }
}
