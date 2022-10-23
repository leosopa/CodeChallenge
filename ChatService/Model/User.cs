using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ChatService.Model
{
    public class User
    {
        [Key]
        [DataType("varchar(50)")]
        public string Login { get; set; }

        [DataType("varchar(50)")]
        public string Name { get; set; }

        public string Password { get; set; }

        public byte[] Salt { get; set; }

        public Room? Room { get; set; }

        public User()
        {
            Name = String.Empty;
            this.Room = new Room();
        }

        public User(string login, string name)
        {
            this.Login = login;
            this.Name = name;
        }

    }
}