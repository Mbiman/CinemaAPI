using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name cannot be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Email cannot be empty")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; }
        public string Role { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}