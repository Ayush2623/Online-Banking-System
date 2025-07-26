using ModelLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces;

public interface IDashboardRepository
    {
        Task<DashboardData> GetDashboardDataAsync(long accountNumber);
        Task<AccountSummary> GetAccountSummaryAsync(long accountNumber);
        Task<List<AccountStatement>> GetAccountStatementAsync(long accountNumber, DateTime startDate, DateTime endDate);
        Task<List<Transaction>> GetAllTransactionsAsync(long accountNumber);
        Task<bool> ChangePasswordAsync(long accountNumber, string oldPassword, string newPassword);
    }
