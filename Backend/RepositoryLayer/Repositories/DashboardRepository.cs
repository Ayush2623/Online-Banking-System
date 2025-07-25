using Microsoft.EntityFrameworkCore;
using ModelLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories;

public class DashboardRepository : IDashboardRepository
    {
        private readonly OnlineBankDbContext _context;

        public DashboardRepository(OnlineBankDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardData> GetDashboardDataAsync(long accountNumber)
        {
            return await _context.Accounts
                .Where(u => u.AccountNumber == accountNumber)
                .Select(u => new DashboardData
                {
                    Name = u.Name,
                    Email = u.Email,
                    MobileNumber = u.MobileNumber,
                    AadharCardNumber = u.AadharCardNumber,
                    ResidentialAddress = u.ResidentialAddress,
                    PermanentAddress = u.PermanentAddress,
                    Occupation = u.OccupationDetails
                })
                .FirstOrDefaultAsync();
        }
         public async Task<AccountSummary> GetAccountSummaryAsync(long accountNumber)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
            if (account == null) return null;

            var transactions = await _context.Transactions
                .Where(t => t.FromAccountNumber == account.AccountNumber)
                .OrderByDescending(t => t.TransactionDate)
                .Take(5)
                .ToListAsync();

            return new AccountSummary
            {
                AccountNumber = account.AccountNumber.ToString(), // Convert long to string
                Balance = account.Balance,
                RecentTransactions = transactions.Select(t => new Transaction
                {
                    TransactionId = t.TransactionId,
                    FromAccountNumber = t.FromAccountNumber,
                    ToAccountNumber = t.ToAccountNumber,
                    Amount = t.Amount,
                    TransactionType = t.TransactionType,
                    TransactionDate = t.TransactionDate,
                    Remarks = t.Remarks
                }).ToList()
            };
        }
        public async Task<List<AccountStatement>> GetAccountStatementAsync(long accountNumber, DateTime startDate, DateTime endDate)
        {
            var accountNumberLong = accountNumber;

            return await _context.Transactions
                .Where(t => (t.FromAccountNumber == accountNumberLong || t.ToAccountNumber == accountNumberLong) 
                            && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .Select(t => new AccountStatement
                {
                    AccountNumber = t.FromAccountNumber == accountNumberLong ? t.FromAccount.AccountNumber.ToString() : t.ToAccount.AccountNumber.ToString(),
                    Name = t.FromAccountNumber == accountNumberLong ? t.FromAccount.Name : t.ToAccount.Name,
                    AccountType = t.FromAccountNumber == accountNumberLong ? t.FromAccount.AccountType : t.ToAccount.AccountType,
                    Balance = t.Amount,
                    TransactionDate = t.TransactionDate,
                    Description = t.Remarks
                })
                .ToListAsync();
        }

        public async Task<bool> ChangePasswordAsync(long accountNumber, string oldPassword, string newPassword)
        {
            var accountNumberLong = accountNumber;

            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.AccountNumber == accountNumberLong && u.Password == oldPassword);
            if (user == null) return false;

            user.Password = newPassword; // Hash the password before saving
            await _context.SaveChangesAsync();
            return true;
        }
    }
