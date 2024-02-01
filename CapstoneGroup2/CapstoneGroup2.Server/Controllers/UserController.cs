using API.Dal;
using API.Model;
using API.Model.Requests;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq;
using LoginRequest = API.Model.Requests.LoginRequest;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly DataContext context;

        public UserController(ILogger<UserController> logger, DataContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        // POST: /user/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                using var connection = new SqlConnection(context.Database.GetConnectionString());
                connection.Open();

                using var command = new SqlCommand("VerifyUser", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@p_username", loginRequest.Username);
                command.Parameters.AddWithValue("@p_password", loginRequest.Password);

                var outputParameter = command.Parameters.Add("@p_result", SqlDbType.Int);
                outputParameter.Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();

                if ((int)outputParameter.Value == 1)
                {
                    return Ok(new { success = true, message = "Login successful" });
                }
                else
                {
                    return Unauthorized(new { success = false, message = "Invalid username or password" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error during login: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred during login" });
            }
        }
    }
}

