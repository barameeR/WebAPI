using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<ActionResult<List<UserModel>>> Get() 
            => await _context.Users.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<List<UserModel>>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
            {
                return BadRequest("User is not found");
            }

            return Ok(user);

        }

        [HttpPost]
        public async Task<ActionResult<List<UserModel>>> AddUser(UserModel user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(await _context.Users.ToListAsync());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<UserModel>>> UpdateUser(int id,UserModel request)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null || user.Id != id)
                return BadRequest($"Not found user with id: {id}");

            user.UserName = request.UserName;
            user.Password = request.Password;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            await _context.SaveChangesAsync();
            return Ok(await _context.Users.FindAsync(id));

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<UserModel>>> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return BadRequest("User is not found");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(await _context.Users.FindAsync(user.Id));
            
        }
    }
}
