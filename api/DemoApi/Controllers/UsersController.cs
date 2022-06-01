
using DemoApi.Data;
using DemoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public UsersController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet("{id}")]
        public ActionResult<UserDto> GetUser(string id)
        {
            User? user = dbContext.Users.FirstOrDefault(x => x.Id == Guid.Parse(id));
            return user == null ? (ActionResult<UserDto>)NotFound() : (ActionResult<UserDto>)Ok(user);
        }

        [HttpPost("create")]
        public ActionResult AddUser(UserDto input)
        {
            User user = Mapper.Map<User>(input);
            _ = dbContext.Users.Add(user);
            _ = dbContext.SaveChanges();

            return Created("", user);
        }

        [HttpGet("users")]
        public ActionResult<List<User>> GetUsers()
        {
            return dbContext.Users.ToList();
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(Guid id)
        {
            User? user = dbContext.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound(id);
            }

            _ = dbContext.Users.Remove(user);
            _ = dbContext.SaveChanges();
            return NoContent();
        }
    }
}