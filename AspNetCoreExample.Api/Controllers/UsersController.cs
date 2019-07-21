using AspNetCoreExample.Api.Models;
using AspNetCoreExample.Infection;

using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreExample.Api.Controllers
{
    [Route("v1/users")]
    public class UsersController : ControllerBase
    {
        [HttpPost, GenerateContracts]
        public ActionResult CreateUsers([FromBody] User[] users)
        {
            return Ok();
        }

        [HttpGet, GenerateContracts]
        public ActionResult<User[]> SearchUsers([FromQuery] string name)
        {
            return Ok();
        }
    }
}