using System.Security.Claims;
using CinemaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CinemaApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SamplesController : ControllerBase
    {

        [Authorize(Roles = "Users")]
        [HttpGet]
        public string Get()
        {
            return "Hello from the userside";
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "Hello from the Adminside";
        }

        [HttpGet("Role")]
        public IActionResult RolesEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.Name}, you are a/an {currentUser.Role}");
        }

        private User GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new User
                {
                    Username = userClaims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value,
                    Email = userClaims.FirstOrDefault(u => u.Type == ClaimTypes.Email)?.Value,
                    Role = userClaims.FirstOrDefault(u => u.Type == ClaimTypes.Role)?.Value,
                    Name = userClaims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value

                };
            }
            return null!;
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {

        }
    }
}