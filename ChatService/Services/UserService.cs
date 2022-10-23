using ChatService.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Services
{
    public class UserService : IUserService
    {

        private readonly ChatDbContext _dbContext;

        public UserService(ChatDbContext context)
        {
            _dbContext = context;
        }
        public Task<User> GetUserByLogin(string userLogin)
        {
            return _dbContext.Users.FirstOrDefaultAsync(u => u.Login == userLogin);
        }
    }
}
