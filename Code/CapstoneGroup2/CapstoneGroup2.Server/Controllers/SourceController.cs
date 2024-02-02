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

    // GET: <SourceController>
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

    // GET <SourceController>/5
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

    // POST <SourceController>
    [HttpPost]
    public IActionResult Create([FromBody] Source shared)
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

    // PUT <SourceController>/5
    [HttpPut]
    public IActionResult Update([FromBody] Source shared)
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

    // DELETE <SourceController>/5
    [HttpDelete]
    public IActionResult Delete([FromBody] Source shared)
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