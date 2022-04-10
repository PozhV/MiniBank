using System;
using System.Threading;
using System.Threading.Tasks;
using MiniBank.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MiniBank.Web.HostedServices
{
    public class MigrationHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public MigrationHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<MiniBankContext>();

                if (context == null)
                {
                    throw new Exception($"{nameof(MiniBankContext)} not registered");
                }

                context.Database.Migrate();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
