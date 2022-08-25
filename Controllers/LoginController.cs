using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CinemaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using AuthenticationPlugin;

namespace CinemaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private CinemaDbContext _dbContext;
        private IConfiguration _config;
   

        public LoginController(CinemaDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody]UserLogin userLogin)
        {
            try
            {
                var user = Authenticate(userLogin);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found. Wrong username/Password");

            }
            catch (System.Exception ex)
            {
                
                return BadRequest(ex.Message);
            }       
        }

        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("kdtmPUb4yIGc5R4ACmC5UaWVfF45Bgp1mMfWZlK4"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username!),
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.Role!)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);
                
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public User Authenticate(UserLogin userLogin)
        {
            var currentUser = _dbContext.Users.FirstOrDefault(u => u.Username!.ToLower() == 
            userLogin.Username!.ToLower());
            if (currentUser == null)
            {
                return null!;
            }
            if (!SecurePasswordHasherHelper.Verify(userLogin.Password, currentUser.Password))
            {
                return null!;

            }else
            {
                return currentUser;
            }

        }
    }

}