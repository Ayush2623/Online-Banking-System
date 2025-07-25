using Microsoft.EntityFrameworkCore;
using ModelLayer.Models;

namespace RepositoryLayer
{
    public class OnlineBankDbContext : DbContext
    {
        public OnlineBankDbContext(DbContextOptions<OnlineBankDbContext> options) : base(options) { }

        public DbSet<Auth> Auths { get; set; }
        public DbSet<PendingAccount> PendingAccounts { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<NetBanking> NetBankings { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Payee> Payees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Auth>()
                .Property(a => a.Role)
                .HasConversion<string>(); // Store Role as a string in the database
            
            modelBuilder.Entity<Auth>()
                .Property(a => a.ResetToken)
                .HasColumnName("ResetToken")
                .IsRequired(false); 

            // Specify precision for decimal fields
            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            // Configure foreign key relationships for Transactions
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.FromAccount)
                .WithMany()
                .HasForeignKey(t => t.FromAccountNumber)
                .HasPrincipalKey(a => a.AccountNumber)
                .OnDelete(DeleteBehavior.Restrict); // Disable cascading delete

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ToAccount)
                .WithMany()
                .HasForeignKey(t => t.ToAccountNumber)
                .HasPrincipalKey(a => a.AccountNumber)
                .OnDelete(DeleteBehavior.Restrict); // Disable cascading delete

            modelBuilder.Entity<Payee>()
            .HasOne(p => p.AccountNumber) // Assuming AccountNumber is the navigation property in Payee
            .WithMany()
            .HasForeignKey(p => p.PayeeAccountNumber)
            .HasPrincipalKey(a => a.AccountNumber)
            .OnDelete(DeleteBehavior.Restrict); // Disable cascading delete
            

            modelBuilder.Entity<NetBanking>()
            .HasOne(nb => nb.Accountnumber)
            .WithMany()
            .HasForeignKey(nb => nb.AccountNumber)
            .HasPrincipalKey(a => a.AccountNumber)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Account>()
            .HasAlternateKey(a => a.AccountNumber);

        }
    }
}