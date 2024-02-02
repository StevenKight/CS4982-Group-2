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

    private readonly IDbDal<Note> context;

    #endregion

    #region Constructors

    public NotesController(ILogger<NotesController> logger, IDbDal<Note> context)
    {
        this.logger = logger;
        this.context = context;
    }

    #endregion

    #region Methods

    // GET: <NotesController>
    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            return Ok(this.context.GetAll());
        }
        catch (UnauthorizedAccessException e)
        {
            this.logger.LogError(e, "Invalid token");
            return Unauthorized("Invalid token");
        }
    }

    // GET <NotesController>/5
    [HttpGet("{id}")] // TODO: User actual key
    public IActionResult GetById(int id)
    {
        try
        {
            return Ok(this.context.Get(id));
        }
        catch (UnauthorizedAccessException e)
        {
            this.logger.LogError(e, "Invalid token");
            return Unauthorized("Invalid token");
        }
    }

    // POST <NotesController>
    [HttpPost]
    public IActionResult Create([FromBody] Note note)
    {
        try
        {
            this.context.Add(note);
            return Ok();
        }
        catch (UnauthorizedAccessException e)
        {
            this.logger.LogError(e, "Invalid token");
            return Unauthorized("Invalid token");
        }
    }

    // PUT <NotesController>/5
    [HttpPut]
    public IActionResult Update([FromBody] Note note)
    {
        try
        {
            this.context.Update(note);
            return Ok();
        }
        catch (UnauthorizedAccessException e)
        {
            this.logger.LogError(e, "Invalid token");
            return Unauthorized("Invalid token");
        }
    }

    // DELETE <NotesController>/5
    [HttpDelete]
    public IActionResult Delete([FromBody] Note note)
    {
        try
        {
            this.context.Delete(note);
            return Ok();
        }
        catch (UnauthorizedAccessException e)
        {
            this.logger.LogError(e, "Invalid token");
            return Unauthorized("Invalid token");
        }
    }

    #endregion
}