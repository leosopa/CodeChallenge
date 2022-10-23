namespace ChatService.Model
{
    public class UserConnection
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Room Room { get; set; }

        public UserConnection()
        {
            this.User = new User();
            this.Room = new Room();
        }

    }
}