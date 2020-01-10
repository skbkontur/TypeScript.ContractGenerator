using System;

using AspNetCoreExample.Api.Models;

using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreExample.Api.Controllers
{
    [Route("v1/user")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public ActionResult CreateUser([FromBody] User user)
        {
            return Ok();
        }

        [HttpDelete("{userId}")]
        public ActionResult DeleteUser(Guid userId)
        {
            return Ok();
        }

        [HttpGet("{userId}")]
        public ActionResult<User> GetUser(Guid userId)
        {
            return Ok(null);
        }
    }
}