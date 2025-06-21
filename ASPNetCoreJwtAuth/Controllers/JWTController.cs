using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ASPNetCoreJwtAuth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JWTController : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public IActionResult PrivateAPI()
        {

            var list = new[]
            {
                new { Name = "This end point is restricted " },
                new { Name = $"Current Logged in User {User.Identity.Name} "}
            }.ToList();

            foreach (var claim in HttpContext.User.Claims)
            {
                list.Add(new { Name = claim.Value });
            }

            return Ok(list);
        }

        [HttpGet]
        public IActionResult PublicAPI()
        {
            var list = new[]
            {
                new { Code = 1, Name = "This end point can be accessed by Public" },
                new { Code = 2, Name = "Whatever" }
            }.ToList();

            return Ok(list);
        }



    }


}

