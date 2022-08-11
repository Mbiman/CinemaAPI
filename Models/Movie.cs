using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApi.Models
{
    public class Movie
    {
       public int id {get; set;} 
       [Required(ErrorMessage ="Name cannot be empty")]
       public string? Name {get; set;}
       [Required(ErrorMessage ="Language cannot be null or empty")]
       public string? Language {get; set;}
       [Required]
       public double? Ratings { get; set; }
       [NotMapped]
       public IFormFile? Image { get; set; }
       public string? ImageUrl { get; set; }
    }
}