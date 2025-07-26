using BussinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BussinessLayer.Services;

public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardData> GetDashboardDataAsync(long accountNumber)
        {
            return await _dashboardRepository.GetDashboardDataAsync(accountNumber);
        }

        public async Task<AccountSummary> GetAccountSummaryAsync(long accountNumber)
        {
            return await _dashboardRepository.GetAccountSummaryAsync(accountNumber);
        }

        public async Task<List<AccountStatement>> GetAccountStatementAsync(long accountNumber, DateTime startDate, DateTime endDate)
        {
            return await _dashboardRepository.GetAccountStatementAsync(accountNumber, startDate, endDate);
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync(long accountNumber)
        {
            return await _dashboardRepository.GetAllTransactionsAsync(accountNumber);
        }

        public async Task<bool> ChangePasswordAsync(long accountNumber, string oldPassword, string newPassword)
        {
            return await _dashboardRepository.ChangePasswordAsync(accountNumber, oldPassword, newPassword);
        }
    }
