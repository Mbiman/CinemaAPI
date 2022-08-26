using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private CinemaDbContext _dbContext;

        public ReservationsController(CinemaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody]Reservation reservationObj)
        {
            try
            {
                reservationObj.ReservationTime = DateTime.Now;
                _dbContext.Reservations.Add(reservationObj);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetReservations()
        {
            try
            {
                var reservations =  from reservation in _dbContext.Reservations
                                join customer in _dbContext.Users on reservation.UserId equals customer.Id
                                join movie in _dbContext.Movies on reservation.MovieId equals movie.Id
                                select new
                                {
                                    Id = reservation.Id,
                                    ReservationTime = reservation.ReservationTime,
                                    CustomerName = customer.Name,
                                    MovieName = movie.Name
                                };

                return Ok(reservations);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public IActionResult GetReservationDetails(int id)
        {
            try
            {
                var reservationDetail =  (from reservation in _dbContext.Reservations
                                join customer in _dbContext.Users on reservation.UserId equals customer.Id
                                join movie in _dbContext.Movies on reservation.MovieId equals movie.Id
                                where reservation.Id == id
                                select new
                                {
                                    Id = reservation.Id,
                                    ReservationTime = reservation.ReservationTime,
                                    CustomerName = customer.Name,
                                    MovieName = movie.Name,
                                    Email = customer.Email,
                                    Qty = reservation.Qty,
                                    Price = reservation.Price,
                                    Phone = reservation.Phone,
                                    PlayingDate = movie.PlayingDate,
                                    PlayingTime = movie.PlayingTime
                                }).FirstOrDefault();

                return Ok(reservationDetail);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var reservation = _dbContext.Reservations.Find(id);
            if (reservation == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                _dbContext.Reservations.Remove(reservation);
                _dbContext.SaveChanges();
                return Ok("record deleted successfully");
            }     
        }


    }
}