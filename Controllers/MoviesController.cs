using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace CinemaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private CinemaDbContext _dbContext;

        public MoviesController (CinemaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        [HttpGet("[action]")]
        public IActionResult AllMovies(string? sort, int? pageNumber, int? pageSize)
        {
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;
            try
            {
                var movies = from movie in _dbContext.Movies
                        select new
                        {
                            Id = movie.Id,
                            Name = movie.Name,
                            Duration = movie.Duration,
                            Language = movie.Language,
                            Rating = movie.Rating,
                            Genre = movie.Genre,
                            ImageUrl = movie.ImageUrl
                        };

                switch (sort)
                {
                    case "desc":
                        return Ok(movies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize).OrderByDescending(m => m.Rating));
                    case "asc":
                        return Ok(movies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize).OrderBy(m => m.Rating));
                    default:
                        return Ok(movies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
                }  
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }


        [Authorize]
        [HttpGet("[action]/{id}")]
        public IActionResult MovieDetails(int id)
        {
            var movie = _dbContext.Movies.Find(id);
            if (movie == null)
            {
                return NotFound("Movie is not available or does not exist");
            }

            return Ok(movie);
        }


        [Authorize(Roles = "Admin")]
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


        [Authorize(Roles = "Admin")]
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
                movie.Description = movieObj.Description;
                movie.Language = movieObj.Language;
                movie.Duration = movieObj.Duration;
                movie.PlayingDate = movieObj.PlayingDate;
                movie.PlayingTime = movieObj.PlayingTime;
                movie.TicketPrice = movieObj.TicketPrice;
                movie.Rating = movieObj.Rating;
                movie.Genre = movieObj.Genre;
                movie.TrailerUrl = movieObj.TrailerUrl;
                
                _dbContext.SaveChanges();
                return Ok("Record updated successfully");
            }   
        }

        [Authorize(Roles = "Admin")]
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