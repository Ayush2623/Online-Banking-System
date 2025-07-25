using BussinessLayer.Interfaces;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTOs;
using ModelLayer.Models;
using RepositoryLayer.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using BussinessLayer.Helpers;

namespace BussinessLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IAuthRepository authRepository, JwtHelper jwtHelper)
        {
            _authRepository = authRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<string> RegisterAsync(AuthDTO authDto)
        {
            if (await _authRepository.UserExistsAsync(authDto.Username))
            {
                return "Error: Username already exists.";
            }

            if (string.IsNullOrEmpty(authDto.MobileNumber))
            {
                return "Error: Mobile number is required.";
            }

            var authModel = new ModelLayer.Models.Auth
            {
                Username = authDto.Username,
                Password = authDto.Password,
                Role = authDto.Role,
                MobileNumber = authDto.MobileNumber,
                ResetToken = string.Empty,
                ResetTokenExpiry = null
            };

            await _authRepository.RegisterUserAsync(authModel);
            int generatedAuthId = authModel.AuthId;
            return generatedAuthId.ToString();
        }

        public async Task<string> LoginAsync(LoginDTO loginDto)
        {
            var user = await _authRepository.GetUserAsync(loginDto.Username);
            if (user == null || user.Password != loginDto.Password)
            {
                return "Invalid username or password.";
            }

            return _jwtHelper.GenerateToken(user.AuthId, user.Username, user.Role);
        }

        public async Task<Auth> GetUserAsync(string username)
        {
            return await _authRepository.GetUserAsync(username);
        }
    }
}