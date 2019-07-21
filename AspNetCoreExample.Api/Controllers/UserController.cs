using System;

using AspNetCoreExample.Api.Models;
using AspNetCoreExample.Infection;

using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreExample.Api.Controllers
{
    [Route("v1/user")]
    public class UserController : ControllerBase
    {
        [HttpPost, GenerateContracts]
        public ActionResult CreateUser([FromBody] User user)
        {
            return Ok();
        }
        
        [HttpDelete("{userId}"), GenerateContracts]
        public ActionResult DeleteUser(Guid userId)
        {
            return Ok();
        }

        [HttpGet("{userId}"), GenerateContracts]
        public ActionResult<User> GetUser(Guid userId)
        {
            return Ok(null);
        }
    }
}