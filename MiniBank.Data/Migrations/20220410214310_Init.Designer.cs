﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MiniBank.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MiniBank.Data.Migrations
{
    [DbContext(typeof(MiniBankContext))]
    [Migration("20220410214310_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MiniBank.Data.Accounts.AccountDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric")
                        .HasColumnName("balance");

                    b.Property<DateTime>("CloseDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("close_date");

                    b.Property<string>("CurrencyName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("currency_name");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("boolean")
                        .HasColumnName("is_open");

                    b.Property<DateTime>("OpenDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("open_date");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_accounts");

                    b.ToTable("accounts", (string)null);
                });

            modelBuilder.Entity("MiniBank.Data.Transactions.TransactionDbModel", b =>
                {
                    b.Property<Guid>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("transaction_id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric")
                        .HasColumnName("amount");

                    b.Property<string>("CurrencyName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("currency_name");

                    b.Property<Guid>("FromAccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("from_account_id");

                    b.Property<Guid>("ToAccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("to_account_id");

                    b.HasKey("TransactionId")
                        .HasName("pk_transaction");

                    b.ToTable("Transaction", (string)null);
                });

            modelBuilder.Entity("MiniBank.Data.Users.UserDbModel", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("login");

                    b.HasKey("UserId")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
