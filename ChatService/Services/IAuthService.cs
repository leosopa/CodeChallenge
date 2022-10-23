using ChatService.Model;

namespace ChatService.Services
{
    public interface IAuthService
    {
        public Task<bool> Create(User user, string password);

        public Task<bool> Login(string login, string password);
    }
}
