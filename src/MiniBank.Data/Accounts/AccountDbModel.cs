using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniBank.Data.Accounts
{
    public class AccountDbModel
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("user_id")]
        public Guid UserId { get; set; }
        [Column("balance")]
        public decimal Balance { get; set; }
        [Column("currency_name")]
        public string CurrencyName { get; set; }
        [Column("is_open")]
        public bool IsOpen { get; set; }
        [Column("open_date")]
        public DateTime OpenDate { get; set; }
        [Column("close_date")]
        public DateTime? CloseDate { get; set; }
        internal class Map : IEntityTypeConfiguration<AccountDbModel>
        {
            public void Configure(EntityTypeBuilder<AccountDbModel> builder)
            {
                builder.ToTable("accounts");
                builder.Property(it => it.Id);
                builder.Property(it => it.Balance);
                builder.Property(it => it.CurrencyName);
                builder.Property(it => it.IsOpen);
                builder.Property(it => it.OpenDate);
                builder.Property(it => it.CloseDate);
                builder.HasKey(it => it.Id);
            }
        }
    }
}
