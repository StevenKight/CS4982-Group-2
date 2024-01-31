using API.Dal;
using API.Model;
using Microsoft.AspNetCore.Mvc;

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

    #endregion
}