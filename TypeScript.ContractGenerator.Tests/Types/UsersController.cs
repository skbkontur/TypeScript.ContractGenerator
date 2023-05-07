using System;

using Microsoft.AspNetCore.Mvc;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Types
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

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