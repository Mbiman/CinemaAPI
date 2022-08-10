using Microsoft.EntityFrameworkCore;

namespace CinemaApi.Models
{
    public class CinemaDbContext: DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) :base(options)
        {

        }
        public DbSet<Movie> Movies {get; set;}
    }
}