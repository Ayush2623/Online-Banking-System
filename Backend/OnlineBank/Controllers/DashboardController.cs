using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BussinessLayer.Interfaces;
using ModelLayer.Models;
using ModelLayer.DTOs;

namespace OnlineBank.Controllers
{
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    // Dashboard Page
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard(long accountNumber)
    {
        var dashboardData = await _dashboardService.GetDashboardDataAsync(accountNumber);
        if (dashboardData == null)
        {
            return NotFound(ApiResponse.Error("Dashboard data not found."));
        }

        return Ok(ApiResponse<object>.SuccessResponse("Dashboard data retrieved successfully.", dashboardData));
    }

    // Account Summary
    [HttpGet("account-summary/account-number")]
    public async Task<IActionResult> GetAccountSummary(long accountNumber)
    {
        var summary = await _dashboardService.GetAccountSummaryAsync(accountNumber);
        if (summary == null)
        {
            return NotFound(ApiResponse.Error("Account summary not found."));
        }

        return Ok(ApiResponse<object>.SuccessResponse("Account summary retrieved successfully.", summary));
    }

    // Account Statement
    [HttpGet("account-statement/Account Number")]
    public async Task<IActionResult> GetAccountStatement(long AccountNumber, DateTime startDate, DateTime endDate)
    {
        var statement = await _dashboardService.GetAccountStatementAsync(AccountNumber, startDate, endDate);
        if (statement == null || !statement.Any())
        {
            return NotFound(ApiResponse.Error("No transactions found for the specified period."));
        }

        return Ok(ApiResponse<object>.SuccessResponse("Account statement retrieved successfully.", statement));
    }

    // All Transactions without date filter
    [HttpGet("transactions/{accountNumber}")]
    public async Task<IActionResult> GetAllTransactions(long accountNumber)
    {
        var transactions = await _dashboardService.GetAllTransactionsAsync(accountNumber);
        return Ok(ApiResponse<List<Transaction>>.SuccessResponse("Transactions retrieved successfully.", transactions));
    }

    // Change Password
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await _dashboardService.ChangePasswordAsync(request.AccountNumber, request.OldPassword, request.NewPassword);
        if (!result)
        {
            return BadRequest(ApiResponse.Error("Failed to change password. Please check your credentials."));
        }

        return Ok(ApiResponse.Success("Password changed successfully."));
    }

    // Session Expired
    [HttpGet("session-expired")]
    public IActionResult SessionExpired()
    {
        return Unauthorized(ApiResponse.Error("Session expired. Please log in again."));
    }
}
}
