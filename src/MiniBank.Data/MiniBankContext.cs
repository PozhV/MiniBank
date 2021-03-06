using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniBank.Data.Users;
using MiniBank.Data.Accounts;
using MiniBank.Data.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EFCore.NamingConventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiniBank.Data
{
    public class MiniBankContext : DbContext
    {
        public DbSet<UserDbModel> Users { get; set; }
        public DbSet<AccountDbModel> Accounts { get; set; }
        public DbSet<TransactionDbModel> Transactions { get; set; }

        public MiniBankContext(DbContextOptions options) : base(options)
        {
        }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MiniBankContext).Assembly);
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.LogTo(Console.WriteLine);
            base.OnConfiguring(optionsBuilder);
        }
    }
    public class CamelCaseHistoryContext : NpgsqlHistoryRepository
    {
        public CamelCaseHistoryContext(HistoryRepositoryDependencies dependencies) : base(dependencies)
        {
        }

        protected override void ConfigureTable(EntityTypeBuilder<HistoryRow> history)
        {
            base.ConfigureTable(history);

            history.Property(h => h.MigrationId).HasColumnName("MigrationId");
            history.Property(h => h.ProductVersion).HasColumnName("ProductVersion");
        }
    }

    public class Factory : IDesignTimeDbContextFactory<MiniBankContext>
    {
        public MiniBankContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../MiniBank.Web"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var options = new DbContextOptionsBuilder()
                .UseNpgsql(config["DataBase:ConnectionString"]).ReplaceService<IHistoryRepository, CamelCaseHistoryContext>()
                .UseSnakeCaseNamingConvention().Options;
            return new MiniBankContext(options);
        }
    }
}