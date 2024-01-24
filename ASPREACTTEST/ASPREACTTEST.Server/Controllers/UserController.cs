using Microsoft.AspNetCore.Mvc;

namespace ASPREACTTEST.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        User user1 = new User
        {
            username = "StevenC",
            password = "1234567890",
            description = "New User"
        };

        User user2 = new User
        {
            username = "StevenK",
            password = "Password",
            description = "Experienced User"
        };

        User user3 = new User
        {
            username = "Aaron",
            password = "0987654321",
            description = "Cool User"
        };

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetUser")]
        public IEnumerable<User> Get()
        {
            IEnumerable<User> users = new List<User> { user1, user2, user3};
            return users;
        }
    }
}
