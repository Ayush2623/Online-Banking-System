using ModelLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface IAccountRepository
    {
        Task AddPendingAccountAsync(PendingAccount pendingAccount); //using
        // Task<PendingAccount> GetPendingAccountByIdAsync(int requestId);
         Task<PendingAccount> GetPendingAccountByIdAsync(int requestId);//using
        Task AddAccountAsync(Account account); //using
        Task<int> UpdateAccountAsync(Account account); //using
        Task<Account> GetAccountByIdAsync(long accountNumber); //using
         Task RemovePendingAccountAsync(PendingAccount pendingAccount); //using
        // Task RemovePendingAccountAsync(int requestId);
        Task UpdatePendingAccountAsync(PendingAccount pendingAccount); //using

        Task<string> GetUserIdByEmailAsync(string email); //using
        Task<bool> SavePasswordResetTokenAsync(int userId, string resetToken);
        Task<bool> ValidateResetTokenAsync(int userId, string resetToken);
        Task<bool> UpdatePasswordAsync(int userId, string hashedPassword);
        // Task<Account> GetAccountByUsernameAsync(string username);
        Task<Account> GetAccountByUserIdAsync(int userId); //using
        Task<List<PendingAccount>> GetAllPendingAccountsAsync(); //using
        Task<int> UpdateUserAsync(Auth user); //using

        Task<Auth> GetAuthByUserIdAsync(int userId); //using
        Task<Account> GetUserDetailsByUserIdAsync(int userId); //using
    }
    
}