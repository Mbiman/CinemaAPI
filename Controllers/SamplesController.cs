using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CinemaApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SamplesController : ControllerBase
    {
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

        [HttpPost]
        public void Post([FromBody] string value)
        {

        }
    }
}