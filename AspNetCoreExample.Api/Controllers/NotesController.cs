using System;

using AspNetCoreExample.Api.Models;

using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreExample.Api.Controllers
{
    [Route("v1/user/{userId}/blog")]
    public class NotesController : ControllerBase
    {
        [HttpPost]
        public ActionResult AddEntry(Guid userId, [FromBody] BlogEntry entry)
        {
            return Ok();
        }

        [HttpPost("batch")]
        public ActionResult AddEntries(Guid userId, [FromBody] BlogEntry[] entries)
        {
            return Ok();
        }
    }
}