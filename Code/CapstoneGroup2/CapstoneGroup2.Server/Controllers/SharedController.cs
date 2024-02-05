using CapstoneGroup2.Server.Dal;
using CapstoneGroup2.Server.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CapstoneGroup2.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class SharedController : ControllerBase
{
    #region Data members

    private readonly IDbDal<Shared> context;

    #endregion

    #region Constructors

    public SharedController(IDbDal<Shared> context)
    {
        this.context = context;
    }

    #endregion

    #region Methods

    // GET: <SharedController>
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

    // GET <SharedController>/5
    [HttpGet("{sourceId}-{username}")]
    public IActionResult GetById(int sourceId, string username)
    {
        try
        {
            return Ok(this.context.Get(sourceId, username));
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized("Invalid token");
        }
    }

    // POST <SharedController>
    [HttpPost]
    public IActionResult Create([FromBody] Shared shared)
    {
        try
        {
            this.context.Add(shared);
            return Ok();
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized("Invalid token");
        }
    }

    // PUT <SharedController>/5
    [HttpPut]
    public IActionResult Update([FromBody] Shared shared)
    {
        try
        {
            this.context.Update(shared);
            return Ok();
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized("Invalid token");
        }
    }

    // DELETE <SharedController>/5
    [HttpDelete]
    public IActionResult Delete([FromBody] Shared shared)
    {
        try
        {
            this.context.Delete(shared);
            return Ok();
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized("Invalid token");
        }
    }

    #endregion
}