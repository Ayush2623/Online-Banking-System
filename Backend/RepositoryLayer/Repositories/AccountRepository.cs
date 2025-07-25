using Microsoft.EntityFrameworkCore;
using ModelLayer.Models;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly OnlineBankDbContext _context;

        public AccountRepository(OnlineBankDbContext context)
        {
            _context = context;
        }

        public async Task AddPendingAccountAsync(PendingAccount pendingAccount)
        {
            pendingAccount.Status = "Pending";
            pendingAccount.CreatedAt = DateTime.UtcNow;

            _context.PendingAccounts.Add(pendingAccount);
            await _context.SaveChangesAsync();
        }

        public async Task<PendingAccount> GetPendingAccountByIdAsync(int requestId)
        {
            return await _context.PendingAccounts.FindAsync(requestId);
        }
        public async Task<Account> GetAccountByUserIdAsync(int userId)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId);
        }
        public async Task AddAccountAsync(Account account)
        {
            if (string.IsNullOrEmpty(account.Password))
            {
                account.Password = "DefaultPassword"; // Replace with a hashed default password if necessary
            }
            // if (string.IsNullOrEmpty(account.ResetToken))
            // {
            //     account.ResetToken = string.Empty; // Set a default value for ResetToken
            // }
            Console.WriteLine($"Saving IsNetBankingEnabled: {account.IsNetBankingEnabled}");
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task<Account> GetAccountByIdAsync(long accountNumber)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
        }

        public async Task<int> UpdateAccountAsync(Account account)
        {
            Console.WriteLine($"Saving IsNetBankingEnabled: {account.IsNetBankingEnabled}");
            _context.Accounts.Update(account);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateUserAsync(Auth user)
        {
            var existingUser = await _context.Auths.FirstOrDefaultAsync(u => u.AuthId == user.AuthId);
            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }

            // Update only the fields that are being changed
            existingUser.ResetToken = user.ResetToken;
            existingUser.ResetTokenExpiry = user.ResetTokenExpiry;

            return await _context.SaveChangesAsync(); // Save changes to the database
        }
        public async Task RemovePendingAccountAsync(PendingAccount pendingAccount)
        {
            _context.PendingAccounts.Remove(pendingAccount);
            await _context.SaveChangesAsync();
        }
        public async Task UpdatePendingAccountAsync(PendingAccount pendingAccount)
        {
            _context.PendingAccounts.Update(pendingAccount);
            await _context.SaveChangesAsync();
        }
        public async Task<string> GetUserIdByEmailAsync(string email)
        {
            var user = await _context.Accounts
                .Where(u => u.Email == email)
                .Select(u => u.UserId.ToString())
                .FirstOrDefaultAsync();

            return user;
        }

        // Save Password Reset Token
        public async Task<bool> SavePasswordResetTokenAsync(int userId, string resetToken)
        {
            var user = await _context.Auths.FirstOrDefaultAsync(u => u.AuthId == userId);
            if (user == null)
            {
                return false;
            }

            user.ResetToken = resetToken;
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1); // Token valid for 1 hour
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ValidateResetTokenAsync(int userId, string resetToken)
        {
            var user = await _context.Auths.FirstOrDefaultAsync(u => u.AuthId == userId && u.ResetToken == resetToken);
            if (user == null || user.ResetTokenExpiry < DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }

        // Update Password
        public async Task<bool> UpdatePasswordAsync(int userId, string hashedPassword)
        {
            var user = await _context.Auths.FirstOrDefaultAsync(u => u.AuthId == userId);
            if (user == null)
            {
                return false;
            }

            user.Password = hashedPassword;
            user.ResetToken = null; // Clear the reset token
            user.ResetTokenExpiry = null;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<PendingAccount>> GetAllPendingAccountsAsync()
        {
            return await _context.PendingAccounts
        .Where(pa => pa.Status == "Pending") // Fetch only pending requests
        .ToListAsync();
        }

  
        public async Task<Auth> GetAuthByUserIdAsync(int userId)
        {
            return await _context.Auths.FirstOrDefaultAsync(a => a.AuthId == userId);
        }

        public async Task<Account> GetUserDetailsByUserIdAsync(int userId)
        {
            return await _context.Accounts.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        
    }
}