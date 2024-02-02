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
            this.logger.LogError(e, "Invalid token");
            return Unauthorized("Invalid token");
        }
    }

    // GET <SharedController>/5
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
            this.logger.LogError(e, "Invalid token");
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
            this.logger.LogError(e, "Invalid token");
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
            this.logger.LogError(e, "Invalid token");
            return Unauthorized("Invalid token");
        }
    }

    #endregion
}