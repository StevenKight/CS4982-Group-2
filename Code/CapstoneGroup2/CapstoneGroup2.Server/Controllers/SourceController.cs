using API.Dal;
using API.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class SourceController : ControllerBase
{
    #region Data members

    private readonly ILogger<SourceController> logger;

    private readonly IDbDal<Source> context;

    #endregion

    #region Constructors

    public SourceController(ILogger<SourceController> logger, IDbDal<Source> context)
    {
        this.logger = logger;
        this.context = context;
    }

    #endregion

    #region Methods

    // GET: <NotesController>
    [HttpGet]
    public IEnumerable<Source> Get()
    {
        return this.context.GetAll();
    }

    #endregion
}