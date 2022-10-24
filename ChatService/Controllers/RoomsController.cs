using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatService.Model;

namespace ChatService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : Controller
    {
        private readonly ChatDbContext _context;

        public RoomsController(ChatDbContext context)
        {
            _context = context;
        }

        // GET: api/Rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            return await _context.Rooms.ToListAsync();
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(string id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

        // PUT: api/Rooms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(string id, Room room)
        {
            if (id != room.Name)
            {
                return BadRequest();
            }

            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpGet("create")]
        public IActionResult CreateRoom()
        {
            return View();
        }

        // POST: api/Rooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create")]
        public async Task<ActionResult<Room>> CreateRoom([FromForm] string name)
        {
            var room = new Room() { Name = name };
            _context.Rooms.Add(room);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RoomExists(room.Name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRoom", new { id = room.Name }, room);
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(string id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("joinroom/{id}/{username}")]
        public async Task<ActionResult<Room>> JoinRoom(string roomName, string userLogin)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == userLogin);

            if (user == null)
                return BadRequest();
            else if (user.Room != null && !user.Room.Name.Equals(String.Empty))
                return BadRequest("User already joined!");

            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Name == roomName);

            if (room == null)
                return BadRequest();
            else
            {
                _context.Entry(room).Collection(r => r.Users).Load();
                _context.Entry(room).Collection(r => r.Messages).Load();
                room.Users.Add(user);
            }

            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(roomName))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return room;
        }

        [HttpPut("sendmessage/{id}/{user}")]
        public async Task<ActionResult<Message>> SendMessage(string roomName, string user, Message message)
        {

            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Name == roomName);
            var _user = await _context.Users.FirstOrDefaultAsync(u => u.Login == user);

            if (room == null)
                return BadRequest();
            else
                message.RoomName = room.Name;

            if (_user == null)
                return BadRequest();
            else
                message.UserLogin = _user.Login;

            if (room == null)
                return BadRequest();
            else
                room.Messages.Add(message);

            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(roomName))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return message;
        }

        [HttpPut("leaveroom/{id}/{user}")]
        public async Task<IActionResult> LeftRoom(string roomName, string user)
        {

            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Name == roomName);
            var removeUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == user);

            if (room == null || removeUser == null)
                return BadRequest();
            else
                room.Users.Remove(removeUser);

            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(roomName))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool RoomExists(string id)
        {
            return _context.Rooms.Any(e => e.Name == id);
        }
    }
}
