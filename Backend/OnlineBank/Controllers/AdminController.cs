using BussinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs;
using System.Collections.Generic;
 
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAccountService _accountService;
 
    public AdminController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    [HttpGet("pending-requests")]
    public async Task<IActionResult> GetPendingRequests()
    {
        var pendingRequests = await _accountService.GetPendingRequestsAsync();
        return Ok(ApiResponse<object>.SuccessResponse("Pending requests retrieved successfully.", pendingRequests));
    } 

    [HttpPost("approve-account/{requestId}")]
    public async Task<IActionResult> ApproveAccount(int requestId)
    {
        var result = await _accountService.ApproveAccountAsync(requestId);
        if (result.StartsWith("Success"))
        {
            return Ok(ApiResponse.Success("Account approved successfully."));
        }
        return BadRequest(ApiResponse.Error(result));
    }

    // Reject account opening request
    [HttpPost("reject-account/{requestId}")]
    public async Task<IActionResult> RejectAccount(int requestId)
    {
        var result = await _accountService.RejectAccountAsync(requestId);
        if (result.StartsWith("Success"))
        {
            return Ok(ApiResponse.Success("Account rejected successfully."));
        }
        return BadRequest(ApiResponse.Error(result));
    }
}