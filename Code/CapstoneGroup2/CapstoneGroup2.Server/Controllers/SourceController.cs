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

    private readonly IDbDal<Source> dal;

    #endregion

    #region Constructors

    public SourceController(IDbDal<Source> dal)
    {
        this.dal = dal;
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

        this.dal.SetUser(username);

        return Ok(this.dal.GetAll());
    }

    // GET <SourceController>/5-username
    [HttpGet("{sourceId}-{username}")]
    public IActionResult GetById(int sourceId, string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized("Invalid username");
        }

        this.dal.SetUser(username);

        try
        {
            return Ok(this.dal.Get(sourceId));
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

        this.dal.SetUser(username);

        try
        {
            return Ok(this.dal.Add(newSource));
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

        this.dal.SetUser(username);

        try
        {
            this.dal.Update(shared);
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
            return Ok(this.dal.Delete(shared));
        }
        catch (Exception e)
        {
            return BadRequest("Invalid delete: " + e.Message);
        }
    }

    // GET <SourceController>/5-username
    [HttpPost("Tag/{username}")]
    public IActionResult GetByTagId(string username, [FromBody] List<Tag> tags)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized("Invalid username");
        }

        this.dal.SetUser(username);

        try
        {
            return Ok((this.dal as SourceDal).GetSourcesByTags(tags));
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
    #endregion
}