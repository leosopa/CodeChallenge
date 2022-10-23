using System.ComponentModel.DataAnnotations;

namespace ChatService.Model
{
    public class Room
    {
        [Key]
        [DataType("varchar[50]")]
        public string Name { get; set; }

        public List<User> Users { get; set; }

        public List<Message> Messages { get; set; }

        public Room()
        {
            this.Messages = new List<Message>();
            this.Users = new List<User>();
        }

        public Room(string name)
        {
            this.Name = name;
            this.Users = new List<User>();
            this.Messages = new List<Message>();
        }
    }
}