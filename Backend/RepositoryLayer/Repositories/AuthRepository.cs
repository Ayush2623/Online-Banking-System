using Microsoft.EntityFrameworkCore;
using ModelLayer.DTOs;
using ModelLayer.Models;
using RepositoryLayer.Interfaces;
// using ModelLayer.Enums;

namespace RepositoryLayer.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly OnlineBankDbContext _context;

        public AuthRepository(OnlineBankDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Auths.AnyAsync(a => a.Username == username);
        }

        public async Task RegisterUserAsync(Auth auth)
        {
            _context.Auths.Add(auth);
            await _context.SaveChangesAsync();
        }
        public async Task<Auth> GetUserAsync(string username)
        {
            return await _context.Auths.FirstOrDefaultAsync(a => a.Username == username);
        }
    }
}