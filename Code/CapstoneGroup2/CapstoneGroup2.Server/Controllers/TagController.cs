using CapstoneGroup2.Server.Dal;
using CapstoneGroup2.Server.Model;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneGroup2.Server.Controllers;
/// <summary>
/// Class for controlling tag logic
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
[Route("[controller]")] // tag/
[ApiController]
public class TagController: ControllerBase
{
    /// <summary>
    /// The dal
    /// </summary>
    private readonly IDbDal<Tag> dal;


    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TagController"/> class.
    /// </summary>
    /// <param name="dal">The dal.</param>
    public TagController(IDbDal<Tag> dal)
    {
        this.dal = dal;
    }

    #endregion

    #region Methods

    // GET: <TagController>/username
    /// <summary>
    /// Gets all.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <returns></returns>
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

    // GET <TagController>/5-username
    /// <summary>
    /// Gets the by identifier.
    /// </summary>
    /// <param name="sourceId">The source identifier.</param>
    /// <param name="username">The username.</param>
    /// <returns></returns>
    [HttpGet("{tagId}-{username}")]
    public IActionResult GetById(int tagId, string username)
    {
        if (tagId < 0)
        {
            return BadRequest("Invalid Tag Id");
        }
        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized("Invalid username");
        }

        this.dal.SetUser(username);

        try
        {
            return Ok(this.dal.Get(tagId));
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }

    // POST <TagController>/username
    /// <summary>
    /// Creates the specified username.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="newTag">The new tag.</param>
    /// <returns></returns>
    [HttpPost("{username}")]
    public IActionResult Create(string username, [FromBody] Tag newTag)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized("Invalid username");
        }

        this.dal.SetUser(username);

        try
        {
            return Ok(this.dal.Add(newTag));
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }

    // PUT <TagController>/username
    /// <summary>
    /// Updates the specified username.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="shared">The shared.</param>
    /// <returns></returns>
    [HttpPut("{username}")]
    public IActionResult Update(string username, [FromBody] Tag shared)
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

    // DELETE <TagController>/5
    /// <summary>
    /// Deletes the specified tag identifier.
    /// </summary>
    /// <param name="tagId">The tag identifier.</param>
    /// <returns></returns>
    [HttpDelete("{tagId}")]
    public IActionResult Delete(int tagId)
    {
        if (tagId < 0)
        {
            return BadRequest("Invalid TagId");
        }
        try
        {
            var shared = new Tag { TagID = tagId };
            return Ok(this.dal.Delete(shared));
        }
        catch (Exception e)
        {
            return BadRequest("Invalid delete: " + e.Message);
        }
    }
    #endregion
}