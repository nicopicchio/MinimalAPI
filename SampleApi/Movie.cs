using Microsoft.EntityFrameworkCore;

namespace SampleApi
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }

        public Movie(string title, string genre, int year)
        {
            Title = title;
            Genre = genre;
            Year = year;
        }
    }
    class MovieDb : DbContext
    {
        public MovieDb(DbContextOptions options) : base(options) { }
        public DbSet<Movie> Movies { get; set; } = null!;
    }
}
