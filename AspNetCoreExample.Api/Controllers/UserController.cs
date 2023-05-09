using AspNetCoreExample.Api.Models;

using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreExample.Api.Controllers;

[Route("v1/users")]
public class UserController : ControllerBase
{
    [HttpPost]
    public ActionResult CreateUser([FromBody] User user)
    {
        return Ok();
    }

    [HttpDelete("{userId:guid}")]
    public ActionResult DeleteUser(Guid userId)
    {
        return Ok();
    }

    [HttpGet("{userId:guid}")]
    public ActionResult<User> GetUser(Guid userId)
    {
        return Ok(null);
    }

    [HttpGet]
    public ActionResult<User[]> SearchUsers([FromQuery] string name)
    {
        return Ok(TotallyNotHttpMethod(name));
    }

    private User[] TotallyNotHttpMethod(string name)
    {
        return Array.Empty<User>();
    }
}