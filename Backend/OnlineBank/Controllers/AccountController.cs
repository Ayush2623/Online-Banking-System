using BussinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs;
using ModelLayer.Models;
using System.Security.Claims;

[ApiController]
// [Authorize]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IAuthService _authService;
    private readonly ISmtpService _smtpService;

    public AccountController(IAccountService accountService, ISmtpService smtpService, IAuthService authService)
    {
        _accountService = accountService;
        _smtpService = smtpService;
        _authService = authService;
    }
    // Submit account opening request
    [HttpPost("open-account")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> OpenAccount([FromBody] PendingAccountDTO pendingAccountDto)
    {
        // Console.WriteLine($"EnableNetBanking: {pendingAccountDto.EnableNetBanking}"); // Debugging log
        var result = await _accountService.OpenAccountAsync(pendingAccountDto);
        if (result.StartsWith("Success"))
        {
            return Ok(ApiResponse.Success("Account opening request submitted successfully."));
        }
        return BadRequest(ApiResponse.Error(result));
    }

    [HttpGet("view-account/{userId}")]
    [Authorize]
    public async Task<IActionResult> ViewAccountByUserId(int userId)
    {
        // Retrieve the logged-in user's ID from the claims
        var loggedInUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Check if the claim is null or invalid
        if (string.IsNullOrEmpty(loggedInUserIdClaim) || !int.TryParse(loggedInUserIdClaim, out var loggedInUserId))
        {
            return Unauthorized(ApiResponse.Error("Invalid or missing user ID in token."));
        }

        // Compare the logged-in user's ID with the requested userId
        if (loggedInUserId != userId)
        {
            return StatusCode(StatusCodes.Status403Forbidden, ApiResponse.Error("You are not authorized to view this account."));
        }

        // Retrieve the account details
        var account = await _accountService.ViewAccountByUserIdAsync(userId);
        if (account == null)
        {
            return NotFound(ApiResponse.Error("Account not found."));
        }

        return Ok(ApiResponse<object>.SuccessResponse("Account retrieved successfully.", account));
    }

    // Update account details
    [HttpPut("update-account/ Account number")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateAccount(long accountNumber, [FromBody] UpdateAccountDTO updatedAccount)
    {
        
        if (accountNumber <= 0)
        {
            return BadRequest(ApiResponse.Error("Account number is required."));
        }

        var result = await _accountService.UpdateAccountAsync(accountNumber, updatedAccount);
        if (!result)
        {
            return NotFound(ApiResponse.Error("Account not found or update failed."));
        }

        return Ok(ApiResponse.Success("Account updated successfully."));
    }


   [HttpPost("forgot-password")]

    public async Task<IActionResult> ForgotPassword([FromBody] ModelLayer.DTOs.ForgotPasswordRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.MobileNumber) || string.IsNullOrEmpty(request.Email))
        {
            return BadRequest(ApiResponse.Error("Invalid request. MobileNumber and Email are required."));
        }

        Console.WriteLine($"Processing ForgotPassword for UserId: {request.UserId}, MobileNumber: {request.MobileNumber}");

        // Verify the mobile number
        var isMobileNumberValid = await _accountService.VerifyMobileNumberAsync(request.UserId, request.MobileNumber);
        if (!isMobileNumberValid)
        {
            Console.WriteLine("Mobile number validation failed.");
            return BadRequest(ApiResponse.Error("Invalid mobile number for the provided user ID."));
        }

        // Generate a reset token
        var resetToken = Guid.NewGuid().ToString();
        var tokenSaved = await _accountService.SaveResetTokenAsync(request.UserId, resetToken);
        if (!tokenSaved)
        {
            // Console.WriteLine("Failed to save reset token.");
            return BadRequest(ApiResponse.Error("Failed to generate reset token. Please try again."));
        }

        // Send the reset token via email
        var resetLink = $"https://yourapp.com/reset-password?token={resetToken}";
        var emailBody = $"Click the following link to reset your password: {resetLink}";
        await _smtpService.SendEmailAsync(request.Email, "Password Reset Request", emailBody);

        Console.WriteLine("Password reset link sent successfully.");
        return Ok(ApiResponse.Success("Password reset link has been sent to your registered email."));
    }

    [HttpPost("forgot-user-id")]
    public async Task<IActionResult> ForgotUserId([FromBody] ModelLayer.DTOs.ForgotUserIdRequest request)
    {
        // Verify the mobile number
        var isMobileNumberValid = await _accountService.VerifyMobileNumberAsync(request.UserId, request.MobileNumber);
        if (!isMobileNumberValid)
        {
            return BadRequest(ApiResponse.Error("Invalid mobile number for the provided username."));
        }

        // Retrieve the user ID
        var userId = await _accountService.GetUserIdByEmailAsync(request.Email);
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest(ApiResponse.Error("User ID not found."));
        }

        // Send the user ID via email
        var emailBody = $"Your User ID is: {userId}";
        await _smtpService.SendEmailAsync(request.Email, "Your User ID", emailBody);

        return Ok(ApiResponse.Success("Your User ID has been sent to your registered email."));
    }
 // Set New Password
   [HttpPost("set-new-password")]
    public async Task<IActionResult> SetNewPassword([FromBody] SetNewPasswordRequest request)
    {
        var result = await _accountService.SetNewPasswordAsync(request.UserId, request.NewPassword, request.ResetToken);
        if (!result)
        {
            return BadRequest(ApiResponse.Error("Failed to set new password. Please ensure the reset token is valid."));
        }

        return Ok(ApiResponse.Success("Password updated successfully."));
    }

}