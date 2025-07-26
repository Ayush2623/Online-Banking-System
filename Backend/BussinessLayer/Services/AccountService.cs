using BussinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Interfaces;
using BCrypt.Net;
using ModelLayer.DTOs;

namespace BussinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISmtpService _smtpSerive;

        public AccountService(IAccountRepository accountRepository, ISmtpService smtpService)
        {
            _accountRepository = accountRepository;
             _smtpSerive = smtpService;
        }

        public async Task<string> OpenAccountAsync(PendingAccountDTO pendingAccount)
        {
            // Use the injected IHttpContextAccessor to access the HttpContext
            var pendingAccountModel = new PendingAccount
            {
                UserId = pendingAccount.UserId,
                Name = pendingAccount.Name,
                Email = pendingAccount.Email,
                MobileNumber = pendingAccount.MobileNumber,
                AadharCardNumber = pendingAccount.AadharCardNumber,
                ResidentialAddress = pendingAccount.ResidentialAddress,
                PermanentAddress = pendingAccount.PermanentAddress,
                OccupationDetails = pendingAccount.OccupationDetails,
                AccountType = pendingAccount.AccountType,
                Balance = pendingAccount.Balance,
                IsNetBankingEnabled = pendingAccount.EnableNetBanking,
                CreatedAt = DateTime.UtcNow
            };

            await _accountRepository.AddPendingAccountAsync(pendingAccountModel);
            return $"Account opening request submitted successfully. Your request ID is {pendingAccountModel.RequestId}.";
        }

        public async Task<string> ApproveAccountAsync(int requestId)
        {
            var pendingAccount = await _accountRepository.GetPendingAccountByIdAsync(requestId);
            if (pendingAccount == null)
            {
                return "Pending account request not found.";
            }
            var account = new Account
            {
                UserId = pendingAccount.UserId, // Replace with actual user ID
                AccountNumber = GenerateNumericAccountNumber(),
                Name = pendingAccount.Name,
                Email = pendingAccount.Email,
                MobileNumber = pendingAccount.MobileNumber,
                AadharCardNumber = pendingAccount.AadharCardNumber,
                ResidentialAddress = pendingAccount.ResidentialAddress,
                PermanentAddress = pendingAccount.PermanentAddress,
                OccupationDetails = pendingAccount.OccupationDetails,
                AccountType = pendingAccount.AccountType,
                Balance = pendingAccount.Balance,
                CreatedAt = DateTime.UtcNow,
                IsNetBankingEnabled = pendingAccount.IsNetBankingEnabled,
                // UpdatedAt = DateTime.UtcNow,
                // Otp = string.Empty, // Set default value for Otp
                // OtpExpiry = null
            };

            await _accountRepository.AddAccountAsync(account);
            await _accountRepository.RemovePendingAccountAsync(pendingAccount);
            return "Account approved and created successfully.";
        }
        public async Task<string> RejectAccountAsync(int requestId)
        {
            var request = await _accountRepository.GetPendingAccountByIdAsync(requestId);
            if (request == null || request.Status != "Pending")
            {
                return "Request not found or already processed.";
            }

            request.Status = "Rejected";
            await _accountRepository.UpdatePendingAccountAsync(request);
            return "Account request rejected.";
        }
        public async Task<Account> ViewAccountAsync(long accountNumber)
        {
            return await _accountRepository.GetAccountByIdAsync(accountNumber);
        }
        public async Task<bool> UpdateAccountAsync(long accountNumber, UpdateAccountDTO updatedAccount)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountNumber);
            if (account == null)
            {
                Console.WriteLine($"Account not found for AccountNumber: {accountNumber}");
                return false; // Account not found
            }

            // Update the account details
            account.Name = updatedAccount.Name;
            account.Email = updatedAccount.Email;
            account.MobileNumber = updatedAccount.MobileNumber;
            account.ResidentialAddress = updatedAccount.ResidentialAddress;
            account.PermanentAddress = updatedAccount.PermanentAddress;
            account.OccupationDetails = updatedAccount.OccupationDetails;
            account.IsNetBankingEnabled = updatedAccount.EnableNetBanking;
            // account.AccountType = updatedAccount.AccountType;

            var result = await _accountRepository.UpdateAccountAsync(account);
            return result > 0; // Return true if the update was successful
        }
        public async Task<string> GetUserIdByEmailAsync(string email)
        {
            return await _accountRepository.GetUserIdByEmailAsync(email);
        }
        public async Task<bool> SetNewPasswordAsync(int UserId, string newPassword, string resetToken)
        {
            var user = await _accountRepository.GetAuthByUserIdAsync(UserId);
            if (user == null || user.ResetToken != resetToken || user.ResetTokenExpiry < DateTime.UtcNow)
            {
                return false; // Invalid token or user
            }

            user.Password = newPassword; // Hash the password before saving
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            var result = await _accountRepository.UpdateUserAsync(user);
            return result > 0;
        }

        public async Task<AccountDTO> ViewAccountByUserIdAsync(int userId)

        {
            // Retrieve the account by UserId
            var account = await _accountRepository.GetAccountByUserIdAsync(userId);
            if (account == null)
            {
                return null; // Account not found
            }

            // Map the account to a DTO and return it
            return new AccountDTO
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber, // Map AccountNumber
                Name = account.Name,
                Email = account.Email,
                MobileNumber = account.MobileNumber,
                AadharCardNumber = account.AadharCardNumber,
                ResidentialAddress = account.ResidentialAddress,
                PermanentAddress = account.PermanentAddress,
                OccupationDetails = account.OccupationDetails,
                AccountType = account.AccountType,
                Balance = account.Balance,
                // CreatedAt = account.CreatedAt,
                // UpdatedAt = account.UpdatedAt,
                EnableNetBanking = account.IsNetBankingEnabled // Map EnableNetBanking property
            };

        }
        public async Task<List<PendingAccount>> GetPendingRequestsAsync()
        {
            var pendingAccounts = await _accountRepository.GetAllPendingAccountsAsync();
            return pendingAccounts.Select(pa => new PendingAccount
            {
                UserId=pa.UserId,
                RequestId = pa.RequestId,
                Name = pa.Name,
                Email = pa.Email,
                MobileNumber = pa.MobileNumber,
                AadharCardNumber = pa.AadharCardNumber,
                ResidentialAddress = pa.ResidentialAddress,
                PermanentAddress = pa.PermanentAddress,
                OccupationDetails = pa.OccupationDetails,
                AccountType = pa.AccountType,
                Balance = pa.Balance,
                Status = pa.Status,
                CreatedAt = pa.CreatedAt,
                IsNetBankingEnabled = pa.IsNetBankingEnabled
            }).ToList();
        }
    
       public async Task<bool> VerifyMobileNumberAsync(int userId, string mobileNumber)
        {
            if (userId <= 0 || string.IsNullOrEmpty(mobileNumber))
            {
                Console.WriteLine("Invalid input: UserId or MobileNumber is null or empty.");
                return false; // Invalid input
            }

            // Retrieve user details using UserId
            var userDetails = await _accountRepository.GetUserDetailsByUserIdAsync(userId); // Corrected parameter
            if (userDetails == null)
            {
                Console.WriteLine($"User not found for UserId: {userId}");
                return false; // User not found
            }

            if (userDetails.MobileNumber != mobileNumber)
            {
                Console.WriteLine($"Mobile number mismatch for UserId: {userId}");
                return false; // Mobile number mismatch
            }

            return true;
        }
        public async Task<bool> SaveResetTokenAsync(int userId, string resetToken)
            {
                if (userId <= 0 || string.IsNullOrEmpty(resetToken))
                {
                    Console.WriteLine("Invalid input: UserId or ResetToken is null or empty.");
                    return false; // Invalid input
                }

                // Retrieve user details using UserId
                var userDetails = await _accountRepository.GetUserDetailsByUserIdAsync(userId); // Corrected parameter
                if (userDetails == null)
                {
                    Console.WriteLine($"User not found for UserId: {userId}");
                    return false; // User not found
                }

                // Retrieve the Auth record for the user
                var authUser = await _accountRepository.GetAuthByUserIdAsync(userId); // Corrected parameter
                if (authUser == null)
                {
                    Console.WriteLine($"Auth record not found for UserId: {userId}");
                    return false; // Auth record not found
                }

                // Update the reset token and expiry
                authUser.ResetToken = resetToken;
                authUser.ResetTokenExpiry = DateTime.UtcNow.AddHours(1); // Token valid for 1 hour

                var result = await _accountRepository.UpdateUserAsync(authUser);
                if (result <= 0)
                {
                    Console.WriteLine("Failed to update user with reset token.");
                    return false; // Update failed
                }

                return true;
            }
            private long GenerateNumericAccountNumber()
            {
                var random = new Random();
                var accountNumber = string.Concat(Enumerable.Range(0, 10).Select(_ => random.Next(0, 10).ToString()));
                return long.Parse(accountNumber);
            }
        }
    }
