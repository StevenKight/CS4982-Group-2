using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CapstoneGroup2.Server.Dal;
using CapstoneGroup2.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CapstoneGroup2.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    #region Data members

    private readonly IDbDal<User> context;

    #endregion

    #region Constructors

    public UserController(IDbDal<User> context)
    {
        this.context = context;
    }

    #endregion

    #region Methods

    // GET: <UserController>
    [HttpPost]
    [Route("/login")]
    public IActionResult Login([FromBody] User user)
    {
        if (user == null ||
            string.IsNullOrWhiteSpace(user.Username) ||
            string.IsNullOrWhiteSpace(user.Password))
        {
            return BadRequest();
        }

        User dbUser;
        try
        {
            dbUser = this.context.Get(user.Username);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }

        if (dbUser.Password != user.Password)
        {
            return Unauthorized();
        }

        user.Password = null;

        // Create token with "unique_name": "<USERNAME>"
        var token = new JwtSecurityToken(
            "CapstoneGroup2",
            "CapstoneGroup2",
            new[] { new Claim(JwtRegisteredClaimNames.UniqueName, user.Username) },
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey("CapstoneGroup2SecretKey"u8.ToArray()),
                SecurityAlgorithms.Aes128CbcHmacSha256
            )
        );
        user.Token = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(user);
    }

    // POST <NotesController>
    [HttpPost]
    [Route("/sign-up")]
    public IActionResult AddUser([FromBody] User user)
    {
        if (user == null ||
            string.IsNullOrWhiteSpace(user.Username) ||
            string.IsNullOrWhiteSpace(user.Password))
        {
            return NoContent();
        }

        var users = this.context.GetAll();

        if (users.Any(u => u.Username == user.Username))
        {
            return Conflict();
        }

        try
        {
            this.context.Add(user);

            var loginResult = this.Login(user);
            var newUser = (loginResult as OkObjectResult)?.Value as User;
            return Ok(newUser);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    #endregion
}