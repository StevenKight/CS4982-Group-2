using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Dal;
using API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    #region Data members

    private readonly ILogger<UserController> logger;

    private readonly IDbDal<User> context;

    #endregion

    #region Constructors

    public UserController(ILogger<UserController> logger, IDbDal<User> context)
    {
        this.logger = logger;
        this.context = context;
    }

    #endregion

    #region Methods

    // GET: <NotesController>
    [HttpGet]
    public IEnumerable<User> Get()
    {
        return this.context.GetAll();
    }

    [HttpGet]
    [Route("/login/{username}")]
    public IActionResult Login(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return BadRequest();
        }

        var user = this.context.Get(username);

        if (user == null)
        {
            return NotFound();
        }

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

    #endregion
}