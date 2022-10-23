using ChatService.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Services
{
    public class AuthService : IAuthService
    {
        private readonly ChatDbContext _dbContext;
        public AuthService(ChatDbContext context)
        {
            _dbContext = context;
        }

        public async Task<bool> Create(User user, string password)
        {
            var validateUser = await _dbContext.Users.FindAsync(user.Login);

            if (validateUser != null)
                return false;

            GenerateHash(user, password);

            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Login(string login, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login.ToLower().Equals(login.ToLower()));
            if (user == null)
            {
                return false;
            }
            else if (!VerifyPasswordHash(password, user))
            {
                return false;
            }
            else
            {
                return true;
            }

            
        }

        private bool VerifyPasswordHash(string password, User user)
        {
            var hmac = new System.Security.Cryptography.HMACSHA512(user.Salt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++) {
                if (computedHash[i] != user.Password[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void GenerateHash(User user, string password)
        {
            var hmac = new System.Security.Cryptography.HMACSHA512();

            user.Salt = hmac.Key;
            user.Password = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

        }
    }
}
