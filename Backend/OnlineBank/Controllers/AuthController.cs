using BussinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs;

// using ModelLayer.DTOs;
// Ensure the namespace containing 'Auth' and 'Role' is imported
using ModelLayer.Models; // Add the namespace containing the 'Auth' class

[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthDTO auth)
    {
        var result = await _authService.RegisterAsync(auth);

        if (result.StartsWith("Error:"))
        {
            var errorMessage = result.Substring(7); // Remove "Error: " prefix
            if (errorMessage == "Username already exists.")
                return Conflict(new { message = errorMessage }); // HTTP 409
            if (errorMessage == "Mobile number is required.")
                return BadRequest(new { message = errorMessage }); // HTTP 400
            return BadRequest(new { message = errorMessage }); // Default for other errors
        }

        // If result is not an error, it should be the AuthId
        if (int.TryParse(result, out int authId))
        {
            return Ok(new { message = "User registered successfully.", authId }); // HTTP 200
        }

        // Fallback for unexpected results
        return BadRequest(new { message = "Unexpected result from registration." });
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO login)
    {
        var result = await _authService.LoginAsync(login);

        if (result == "Invalid username or password.")
            return Unauthorized(new { message = result }); // HTTP 401

        // Fetch user details to get AuthId
        var user = await _authService.GetUserAsync(login.Username);
        if (user == null)
            return BadRequest(new { message = "User not found after login." });

        
        return Ok(new { message = "Login successful.", token = result, authId = user.AuthId }); // HTTP 200
    }
}
