using System;

using AspNetCoreExample.Api.Models;
using AspNetCoreExample.Infection;

using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreExample.Api.Controllers
{
    [Route("v1/user/{userId}/blog")]
    public class NotesController : ControllerBase
    {
        [HttpPost, GenerateContracts]
        public ActionResult AddEntry(Guid userId, [FromBody] BlogEntry entry)
        {
            return Ok();
        }
        
        [HttpPost("batch"), GenerateContracts]
        public ActionResult AddEntries(Guid userId, [FromBody] BlogEntry[] entries)
        {
            return Ok();
        }
    }
}