using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatService.Model
{
    public class Message
    {
        public Message()
        {
            //this.User = new User();
            this.Room = new Room();
        }

        public Message(string message)
        {
            this.Text = message;
        }

        [Key]
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime TimeStamp { get; set; }

        
        public string UserLogin { get; set; }

        
        public string RoomName { get; set; }

        [ForeignKey("UserLogin")]
        public User User { get; set; }

        [ForeignKey("RoomName")]
        public Room Room { get; set; }

    }
}