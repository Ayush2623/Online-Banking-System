using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BussinessLayer.Interfaces;
using ModelLayer.Models;

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
            return NotFound("Dashboard data not found.");
        }

        return Ok(dashboardData);
    }

    // Account Summary
    [HttpGet("account-summary/account-number")]
    public async Task<IActionResult> GetAccountSummary(long accountNumber)
    {
        var summary = await _dashboardService.GetAccountSummaryAsync(accountNumber);
        if (summary == null)
        {
            return NotFound("Account summary not found.");
        }

        return Ok(summary);
    }

    // Account Statement
    [HttpGet("account-statement/Account Number")]
    public async Task<IActionResult> GetAccountStatement(long AccountNumber, DateTime startDate, DateTime endDate)
    {
        var statement = await _dashboardService.GetAccountStatementAsync(AccountNumber, startDate, endDate);
        if (statement == null || !statement.Any())
        {
            return NotFound("No transactions found for the specified period.");
        }

        return Ok(statement);
    }

    // Change Password
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await _dashboardService.ChangePasswordAsync(request.AccountNumber, request.OldPassword, request.NewPassword);
        if (!result)
        {
            return BadRequest("Failed to change password. Please check your credentials.");
        }

        return Ok("Password changed successfully.");
    }

    // Session Expired
    [HttpGet("session-expired")]
    public IActionResult SessionExpired()
    {
        return Unauthorized("Session expired. Please log in again.");
    }
}
}
