using System;
using CinemaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AuthenticationPlugin;


namespace CinemaApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private CinemaDbContext _dbContext;
        private IConfiguration _configuration;
        private readonly AuthService _auth;

        public UsersController(CinemaDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }


        [HttpPost]
        public IActionResult Register([FromBody]User user)
        {
           var similarUserEmail = _dbContext.Users.Where(u=>u.Email == user.Email).SingleOrDefault();
           if (similarUserEmail != null)
           {
                return BadRequest("User with same email already exits");
           }
           var userObj = new User
           {
                Name = user.Name,
                Email = user.Email,
                Password = SecurePasswordHasherHelper.Hash(user.Password),
                Role = "Users"
           };
           _dbContext.Users.Add(userObj);
           _dbContext.SaveChanges();
          
           return StatusCode(StatusCodes.Status201Created);
        }

        
    }
}