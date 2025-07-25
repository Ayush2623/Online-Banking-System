using ModelLayer.DTOs;
using ModelLayer.Models;

namespace BussinessLayer.Interfaces
{
    public interface IAccountService
    {
        Task<string> OpenAccountAsync(PendingAccountDTO pendingAccount); //using
        Task<string> ApproveAccountAsync(int requestId); //using
        Task<string> RejectAccountAsync(int requestId); //using
        Task<bool> UpdateAccountAsync(long account, UpdateAccountDTO updatedAccount);//using
        Task<string> GetUserIdByEmailAsync(string email); //using
        Task<AccountDTO> ViewAccountByUserIdAsync(int userId); //using
        Task<List<PendingAccount>> GetPendingRequestsAsync(); //using
        Task<bool> VerifyMobileNumberAsync(int userId, string mobileNumber);//using
        Task<bool> SaveResetTokenAsync(int userId, string resetToken);//using
        Task<bool> SetNewPasswordAsync(int UserId, string newPassword, string resetToken); //using


    }
    
}