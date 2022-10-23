using Microsoft.EntityFrameworkCore;

namespace ChatService.Model
{
    public class ChatDbContext : DbContext
    {

        public DbSet<Room> Rooms { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Message> Messages { get; set; }

        public ChatDbContext(DbContextOptions<ChatDbContext> options) :
            base(options)
        {

        }


    }
}
