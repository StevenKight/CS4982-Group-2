using API.Dal;
using API.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class SharedController : ControllerBase
{
    #region Data members

    private readonly ILogger<SharedController> logger;

    private readonly IDbDal<Shared> context;

    #endregion

    #region Constructors

    public SharedController(ILogger<SharedController> logger, IDbDal<Shared> context)
    {
        this.logger = logger;
        this.context = context;
    }

    #endregion

    #region Methods

    // GET: <NotesController>
    [HttpGet]
    public IEnumerable<Shared> Get()
    {
        return this.context.GetAll();
    }

    #endregion
}