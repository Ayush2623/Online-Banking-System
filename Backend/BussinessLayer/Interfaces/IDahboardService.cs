using ModelLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BussinessLayer.Interfaces;

public interface IDashboardService
    {
        Task<DashboardData> GetDashboardDataAsync(long accountNumber);
        Task<AccountSummary> GetAccountSummaryAsync(long accountNumber);
        Task<List<AccountStatement>> GetAccountStatementAsync(long accountNumber, DateTime startDate, DateTime endDate);
        Task<bool> ChangePasswordAsync(long accountNumber, string oldPassword, string newPassword);
    }
