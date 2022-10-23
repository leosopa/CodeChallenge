using ChatService.Model;
using ChatService.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatService.Hubs
{
    public class ChatHub : Hub
    {
        private readonly User botUser;
        private readonly IDictionary<string, UserConnection> connections;
        private readonly IRoomService _serviceRoom;
        private readonly IUserService _serviceUser;
        private readonly BotService _ServiceBot;

        public ChatHub(IDictionary<string, UserConnection> connections, IServiceProvider sp)
        {
            botUser = new User();
            botUser.Login = "ChatBot";
            this.connections = connections;
            var scope = sp.CreateScope();

            _serviceRoom = new RoomService(scope.ServiceProvider.GetRequiredService<ChatDbContext>());
            _serviceUser = new UserService(scope.ServiceProvider.GetRequiredService<ChatDbContext>());
            _ServiceBot = new BotService();

        }

        public async Task SendMessage(string message)
        {
            if (connections.TryGetValue(Context.ConnectionId, out var userConnection))
            {
                userConnection.Room.Messages.Add(new Message(message));

                await Clients.Group(userConnection.Room.Name).SendAsync("ReceiveMessage", userConnection.User.Login, message);

                await _serviceRoom.SendMessage(userConnection.User.Login, userConnection.Room.Name, message);

                if (message.ToLower().Contains("/stock="))
                {
                    var stock = await _ServiceBot.GetSotck(message.Replace("/stock=", ""));

                    if (stock != null)
                    {
                        message = $"{stock.Symbol} quote is ${stock.Close} per share";
                    }

                    await Clients.Group(userConnection.Room.Name).SendAsync("ReceiveMessage", "ChatBot", message);

                    await _serviceRoom.SendMessage("ChatBot", userConnection.Room.Name, message);

                }
            }
        }

        public async Task JoinRoom(UserConnection userConnection)
        {

            var user = _serviceUser.GetUserByLogin(userConnection.User.Login);

            if (user == null)
                throw new KeyNotFoundException();

            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room.Name);

            connections[Context.ConnectionId] = userConnection;

            await SendConnectedUsers(userConnection.Room.Name);

            await this.Clients.Groups(userConnection.Room.Name).SendAsync("Receivemessage", botUser.Login, 
                $"{userConnection.User.Login} has joined {userConnection.Room.Name}");

            await _serviceRoom.JoinRoom(userConnection.User.Login, userConnection.Room.Name);

            await _serviceRoom.SendMessage(botUser.Login, userConnection.Room.Name, $"{userConnection.User.Login} has joined {userConnection.Room.Name}");

        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (connections.TryGetValue(Context.ConnectionId, out var userConnection))
            {
                _serviceRoom.LeftRoom(userConnection.User.Login, userConnection.Room.Name);
                _serviceRoom.SendMessage(botUser.Login, userConnection.Room.Name, $"{userConnection.User.Login} has left {userConnection.Room.Name}");

                connections.Remove(Context.ConnectionId);
                Clients.Group(userConnection.Room.Name).SendAsync("ReceiveMessage", botUser.Login, $"{userConnection.User.Login} has left");
                SendConnectedUsers(userConnection.Room.Name);
                
            }
            return base.OnDisconnectedAsync(exception);
        }

        public Task SendConnectedUsers(string room)
        {
            var users = connections.Values.Where(c => c.Room.Name == room).Select(c => c.User.Login);

            return Clients.Group(room).SendAsync("UsersInRoom", users);
        }


    }
}
