namespace ChatService.DTO
{
    public class MessageDTO
    {
        public int MessageId { get; set; }
        public string Message { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }

        public int RoomId { get; set; }
        public string RoomName { get; set; } 

    }
}
