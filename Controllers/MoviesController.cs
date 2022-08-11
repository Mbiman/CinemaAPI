using System;
using CinemaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace CinemaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MoviesController : ControllerBase
    {
        private CinemaDbContext _dbContext;

        public MoviesController(CinemaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get api/<MoviesController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_dbContext.Movies);
        }

        // Get api/<MoviesController>/id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var movie = _dbContext.Movies.Find(id);
            if (movie == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                return Ok(movie);
            }
            
        }

        // Get api/<MoviesController>/Test/id
        [HttpGet("[action]/{id}")]
        public int Test(int id)
        {
            return id;
        }


        // [HttpPost]
        // public IActionResult Post([FromBody] Movie movieObj)
        // {
        //     _dbContext.Movies.Add(movieObj);
        //     _dbContext.SaveChanges();
        //     return StatusCode(StatusCodes.Status201Created);
        // }


        [HttpPost]
        public IActionResult Post([FromForm] Movie movieObj)
        {
           var guid = Guid.NewGuid();
           var filePath = Path.Combine("wwwroot", guid+".jpg");
           if (movieObj.Image != null)
           {
                var fileStream = new FileStream(filePath, FileMode.Create);
                movieObj.Image.CopyTo(fileStream);
           }

           movieObj.ImageUrl = filePath.Remove(0,7);
            _dbContext.Movies.Add(movieObj);
            _dbContext.SaveChanges();

           return StatusCode(StatusCodes.Status201Created);
        }



        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] Movie movieObj)
        {
            var movie = _dbContext.Movies.Find(id);
            if (movie == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                var guid = Guid.NewGuid();
                var filePath = Path.Combine("wwwroot", guid+".jpg");
                if (movieObj.Image != null)
                {
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    movieObj.Image.CopyTo(fileStream);
                    movie.ImageUrl = filePath.Remove(0,7);
                }
                
                movie.Name = movieObj.Name;
                movie.Language = movieObj.Language;
                movie.Rating = movieObj.Rating;
                _dbContext.SaveChanges();
                return Ok("Record updated successfully");
            }   
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var movie = _dbContext.Movies.Find(id);
            if (movie == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                _dbContext.Movies.Remove(movie);
                _dbContext.SaveChanges();
                return Ok("record deleted successfully");
            }     
        }
    }
}