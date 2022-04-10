using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniBank.Data.Users
{
    public class UserDbModel
    {
        [Column("user_id")]
        public Guid UserId { get; set; }
        [Column("login")]
        public string Login { get; set; }
        [Column("email")]
        public string Email { get; set; }
        internal class Map : IEntityTypeConfiguration<UserDbModel>
        {
            public void Configure(EntityTypeBuilder<UserDbModel> builder)
            {
                builder.ToTable("UserDb");
                builder.Property(it => it.UserId);
                builder.Property(it => it.Login);

                builder.Property(it => it.Email);
                builder.HasKey(it => it.UserId);
            }
        }
    }
}