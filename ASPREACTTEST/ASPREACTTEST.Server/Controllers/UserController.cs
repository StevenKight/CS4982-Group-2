using Microsoft.AspNetCore.Mvc;

namespace ASPREACTTEST.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private static readonly string[] Names = new[]
{
            "StevenC", "StevenK", "Aaron"
        };

        private static readonly string[] Passwords = new[]
{
            "Password", "1234567890", "0987654321", "qwertyuiop", "asdfghjkl"
        };

        private static readonly string[] Descriptions = new[]
        {
            "Cool User", "New User", "Experienced User", "Intermediate User"
        };

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetUser")]
        public IEnumerable<User> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new User
            {
                username = Names[Random.Shared.Next(Names.Length)],
                password = Passwords[Random.Shared.Next(Passwords.Length)],
                description = Descriptions[Random.Shared.Next(Descriptions.Length)]
            })
            .ToArray();
        }
    }
}
