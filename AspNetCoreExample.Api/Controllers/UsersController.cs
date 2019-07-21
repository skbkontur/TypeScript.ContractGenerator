using AspNetCoreExample.Api.Models;

using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreExample.Api.Controllers
{
    [Route("v1/users")]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        public ActionResult CreateUsers([FromBody] User[] users)
        {
            return Ok();
        }

        [HttpGet]
        public ActionResult<User[]> SearchUsers([FromQuery] string name)
        {
            return Ok();
        }
    }
}