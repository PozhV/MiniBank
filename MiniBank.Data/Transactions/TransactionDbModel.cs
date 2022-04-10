using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiniBank.Data.Transactions
{
    public class TransactionDbModel
    {
        [Column("transaction_id")]
        public Guid TransactionId { get; set; }
        [Column("currency_name")]
        public string CurrencyName { get; set; }
        [Column("amount")]
        public decimal Amount { get; set; }
        [Column("from_account_id")]
        public Guid FromAccountId { get; set; }
        [Column("to_account_id")]
        public Guid ToAccountId { get; set; }
        internal class Map : IEntityTypeConfiguration<TransactionDbModel>
        {
            public void Configure(EntityTypeBuilder<TransactionDbModel> builder)
            {
                builder.ToTable("TransactionDb");

                builder.Property(it => it.TransactionId);
                builder.Property(it => it.CurrencyName);
                builder.Property(it => it.Amount);
                builder.Property(it => it.FromAccountId);
                builder.Property(it => it.ToAccountId);
                builder.HasKey(it => it.TransactionId);
            }
        }
    }
}
