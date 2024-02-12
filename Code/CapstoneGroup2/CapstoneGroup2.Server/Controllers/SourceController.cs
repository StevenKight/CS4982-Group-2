using CapstoneGroup2.Server.Dal;
using CapstoneGroup2.Server.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CapstoneGroup2.Server.Controllers;

[Route("[controller]")] // source/
[ApiController]
public class SourceController : ControllerBase
{
    #region Data members

    private readonly IDbDal<Source> context;

    #endregion

    #region Constructors

    public SourceController(IDbDal<Source> context)
    {
        this.context = context;
    }

    #endregion

    #region Methods

    // GET: <SourceController>/username
    [HttpGet("{username}")]
    public IActionResult GetAll(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized("Invalid username");
        }

        this.context.SetUser(username);

        return Ok(this.context.GetAll());
    }

    // GET <SourceController>/5-username
    [HttpGet("{sourceId}-{username}")]
    public IActionResult GetById(int sourceId, string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized("Invalid username");
        }

        this.context.SetUser(username);

        try
        {
            return Ok(this.context.Get(sourceId));
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }

    // POST <SourceController>/username
    [HttpPost("{username}")]
    public IActionResult Create(string username, [FromBody] Source newSource)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized("Invalid username");
        }

        this.context.SetUser(username);

        try
        {
            return Ok(this.context.Add(newSource));
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }

    // PUT <SourceController>/username
    [HttpPut("{username}")]
    public IActionResult Update(string username, [FromBody] Source shared)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized("Invalid username");
        }

        this.context.SetUser(username);

        try
        {
            this.context.Update(shared);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }

    // DELETE <SourceController>/5
    [HttpDelete("{sourceId}")]
    public IActionResult Delete(int sourceId)
    {
        try
        {
            var shared = new Source { SourceId = sourceId };
            return Ok(this.context.Delete(shared));
        }
        catch (Exception e)
        {
            return BadRequest("Invalid delete: " + e.Message);
        }
    }

    #endregion
}