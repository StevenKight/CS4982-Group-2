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
    [HttpGet("{sourceId}-{username}")]
    public IActionResult GetAll(int sourceId, string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized("Invalid username");
        }

        this.context.SetUser(username);
        this.context.SetSourceId(sourceId);

        try
        {
            var sourceNotes = this.context.GetAll();
            return Ok(sourceNotes);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized("Invalid token");
        }
    }

    // GET <NotesController>/5
    //[HttpGet("{noteId}-{username}")]
    //public IActionResult GetById(int noteId, string username)
    //{
    //    if (string.IsNullOrWhiteSpace(username))
    //    {
    //        return Unauthorized("Invalid username");
    //    }

    //    this.context.SetUser(username);

    //    try
    //    {
    //        return Ok(this.context.Get(noteId));
    //    }
    //    catch (UnauthorizedAccessException e)
    //    {
    //        return Unauthorized("Invalid token");
    //    }
    //}

    // POST <NotesController>
    [HttpPost("{username}")]
    public IActionResult Create(string username, [FromBody] Note note)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized("Invalid username");
        }

        this.context.SetUser(username);

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
    [HttpPut("{username}")]
    public IActionResult Update(string username, [FromBody] Note note)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized("Invalid username");
        }

        this.context.SetUser(username);

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
    [HttpDelete("{sourceId}-{username}")]
    public IActionResult Delete(int sourceId, string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized("Invalid username");
        }

        this.context.SetUser(username);

        try
        {
            var note = this.context.Get(sourceId);
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