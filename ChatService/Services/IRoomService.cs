using ChatService.Model;

namespace ChatService.Services
{
    public interface IRoomService
    {

        public Task<string> JoinRoom(string userName, string roomName);

        public Task<string> SendMessage(string userName, string roomName, string textMessage);

        public Task<string> LeftRoom(string userName, string roomName);


    }
}
