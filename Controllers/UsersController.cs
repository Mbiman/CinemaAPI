using System;
using CinemaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AuthenticationPlugin;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace CinemaApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private CinemaDbContext _dbContext;
       

        public UsersController(CinemaDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
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
                Username = user.Username,
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