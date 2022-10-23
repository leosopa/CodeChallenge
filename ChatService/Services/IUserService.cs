using ChatService.Model;

namespace ChatService.Services
{
    public interface IUserService
    {
        public Task<User> GetUserByName(string userName); 

    }
}
