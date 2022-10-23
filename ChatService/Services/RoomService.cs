using ChatService.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Services
{
    public class RoomService : IRoomService
    {

        private readonly ChatDbContext _dbContext;

        public RoomService(ChatDbContext context)
        {
            _dbContext = context;
        }

        public async Task<string> JoinRoom(string userLogin, string roomName)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == userLogin);

            if (user == null)
                return "User not found!";
            else if (user.Room != null && user.Room.Name != null)
                return $"User already joined in {user.Room.Name} !";

            var room = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.Name == roomName);

            user.Room = room;

            if (room == null)
                return "Room not found";
            else
            {
                room.Users.Add(user);
            }

            _dbContext.Entry(room).State = EntityState.Modified;
            _dbContext.Entry(user).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                {
                    throw;
                }
            }

            return "Joined!";
        }

        public async Task<string> LeftRoom(string userLogin, string roomName)
        {
            var room = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.Name == roomName);
            var removeUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == userLogin);

            await this.SendMessage("ChatBot", roomName, $"{userLogin} has left {roomName}");


            if (room == null || removeUser == null)
                return "User not found!";
            else
                room.Users.Remove(removeUser);

            _dbContext.Entry(room).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return "Left";
        }

        public async Task<string> SendMessage(string userLogin, string roomName, string textMessage)
        {
            var room = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.Name == roomName);
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == userLogin);

            Message message = new Message(textMessage);
            message.TimeStamp = DateTime.Now;

            if (room == null)
                return "Room not found";
            else
                message.RoomName = room.Name;

            if (user == null)
                return "User not found";
            else
                message.UserLogin = user.Login;

            room.Messages.Add(message);

            _dbContext.Entry(room).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return "Message sent!";
        }



    }
}
