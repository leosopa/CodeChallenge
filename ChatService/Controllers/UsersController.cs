using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatService.Model;
using ChatService.Services;

namespace ChatService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly ChatDbContext _context;

        private readonly AuthService _authService;

        public UsersController(ChatDbContext context)
        {
            _context = context;
            _authService = new AuthService(context);
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<User>> GetUser(string login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Login)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // Get: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet("create")]
        public IActionResult CreateUser()
        {
            return View();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateUser(User user, string password)
        public async Task<IActionResult> CreateUser([FromForm] string login, [FromForm] string name, [FromForm] string password)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var created = await _authService.Create(new User { Login = login, Name = name }, password);
                    if (created)
                        return Ok("User created!");
                    else
                        return BadRequest();

                }
                else
                    return BadRequest();
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(new User { Login = login, Name = name });
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet("login/{login}/{password}")]
        public async Task<bool> Login(string login, string password)
        {
            var logged = await _authService.Login(login, password);

            return logged;
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Login == id);
        }
    }
}
