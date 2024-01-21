using API.Dal;
using API.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    #region Data members

    private readonly ILogger<NotesController> logger;

    private readonly DataContext context;

    #endregion

    #region Constructors

    public NotesController(ILogger<NotesController> logger, DataContext context)
    {
        this.logger = logger;
        this.context = context;
    }

    #endregion

    #region Methods

    // GET: <NotesController>
    [HttpGet]
    public IEnumerable<UserNote> Get()
    {
        return this.context.UserNotes;
    }

    #endregion
}