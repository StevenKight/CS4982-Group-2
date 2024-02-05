using CapstoneGroup2.Server.Dal;
using CapstoneGroup2.Server.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CapstoneGroup2.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    #region Data members

    private readonly IDbDal<Note> context;

    #endregion

    #region Constructors

    public NotesController(IDbDal<Note> context)
    {
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
            return Unauthorized("Invalid token");
        }
    }

    // GET <NotesController>/5
    [HttpGet("{sourceId}")]
    public IActionResult GetById(int sourceId)
    {
        try
        {
            return Ok(this.context.Get(sourceId));
        }
        catch (UnauthorizedAccessException e)
        {
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
            return Unauthorized("Invalid token");
        }
    }

    #endregion
}