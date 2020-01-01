using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Core.Entities.Concrete;
using BankBros.Backend.Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace BankBros.Backend.DataAccess.Concrete.EntityFramework.Contexts
{
    public class BankBrosContext : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<BalanceType> BalanceTypes { get; set; }
        public virtual DbSet<CreditRequest> CreditRequests { get; set; }
        public virtual DbSet<CustomerCreditRequest> CustomerCreditRequests { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<OperationClaim> OperationClaims { get; set; }
        public virtual DbSet<TransactionResult> TransactionResults { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<UserLog> UserLogs { get; set; }
        public virtual DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<BillOrganization> BillOrganizations { get; set; }
        public virtual DbSet<CustomerDetail> CustomerDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=bankbros.database.windows.net;Database=BankBrosDB;User ID=bankbrosadmin;Password=!BankBros1918;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.Balance)
                .HasColumnType("decimal(19,4)")
                .IsRequired(true);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Sends)
                .WithOne(e => e.Sender)
                .IsRequired()
                .HasForeignKey(e => e.SenderAccountId);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Receives)
                .WithOne(e => e.Receiver)
                .IsRequired()
                .HasForeignKey(e => e.ReceiverAccountId);

            modelBuilder.Entity<Application>()
                .HasMany(e => e.UserLogs)
                .WithOne(e => e.Application)
                .IsRequired()
                .HasForeignKey(e => e.AppId);

            modelBuilder.Entity<BalanceType>()
                .Property(e => e.Currency)
                .HasColumnType("decimal(19,4)")
                .IsRequired(true);

            modelBuilder.Entity<BalanceType>()
                .HasMany(e => e.Accounts)
                .WithOne(e => e.BalanceType)
                .IsRequired()
                .HasForeignKey(e => e.BalanceTypeId);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Accounts)
                .WithOne(e => e.Customer)
                .IsRequired()
                .HasForeignKey(e => e.CustomerId);

            modelBuilder.Entity<Customer>()
                .HasOne(e => e.CustomerDetails)
                .WithOne(e => e.Customer)
                .HasForeignKey<CustomerDetail>(x => x.Id);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Bills)
                .WithOne(e => e.Customer)
                .IsRequired()
                .HasForeignKey(e => e.CustomerId);

            modelBuilder.Entity<Customer>()
               .HasMany(e => e.CustomerCreditRequests)
               .WithOne(e => e.Customer)
               .IsRequired()
               .HasForeignKey(e => e.CustomerId);

            modelBuilder.Entity<OperationClaim>()
                .HasMany(e => e.UserOperationClaims)
                .WithOne(e => e.OperationClaim)
                .IsRequired(true)
                .HasForeignKey(e => e.OperationClaimId);

            modelBuilder.Entity<TransactionResult>()
                .HasMany(e => e.Transactions)
                .WithOne(e => e.TransactionResult)
                .IsRequired(true)
                .HasForeignKey(e => e.TransactionResultId);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.Amount)
                .HasColumnType("decimal(19,4)")
                .IsRequired(true);

            modelBuilder.Entity<TransactionType>()
                .HasMany(e => e.Transactions)
                .WithOne(e => e.TransactionType)
                .IsRequired(true)
                .HasForeignKey(e => e.TransactionTypeId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserLogs)
                .WithOne(e => e.User)
                .IsRequired(true)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Bill>()
                .Property(e => e.Amount)
                .HasColumnType("decimal(19,4)")
                .IsRequired(true);

            modelBuilder.Entity<BillOrganization>()
                .HasMany(e => e.Bills)
                .WithOne(e => e.Organization)
                .IsRequired(true)
                .HasForeignKey(e => e.OrganizationId);

            modelBuilder.Entity<CreditRequest>()
                .HasMany(e => e.CustomerCreditRequests)
                .WithOne(e => e.CreditRequest)
                .IsRequired()
                .HasForeignKey(e => e.CreditRequestId);
        }

    }
}
