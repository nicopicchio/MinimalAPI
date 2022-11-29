using Microsoft.EntityFrameworkCore;
using SampleApi;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Movies") ?? "Data Source=Movies.db";
builder.Services.AddSqlite<MovieDb>(connectionString);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/welcome", WelcomeMsg());

app.MapGet("/api/movies", GetAllMovies());

app.MapGet("api/movies/{id}", GetMovieById());

app.MapPost("/api/add-new-movie", AddNewMovie());

app.MapPut("/api/movies/{id}", UpdateMovie());

app.MapDelete("/api/movies/{id}", DeleteMovie());

app.Run();

static Func<MovieDb, Movie, Task<IResult>> AddNewMovie()
{
    return async (MovieDb db, Movie movie) =>
    {
        await db.Movies.AddAsync(movie);
        await db.SaveChangesAsync();
        return Results.Created("Movie successfully added", movie);
    };
}

static Func<MovieDb, Task<List<Movie>>> GetAllMovies()
{
    return async (MovieDb db) => await db.Movies.ToListAsync();
}

static Func<string> WelcomeMsg()
{
    return () => Message.WelcomeMsg();
}

static Func<MovieDb, int, Task<Movie>> GetMovieById()
{
    return async (MovieDb db, int id) => await db.Movies.FindAsync(id);
}

static Func<MovieDb, Movie, int, Task<IResult>> UpdateMovie()
{
    return async (MovieDb db, Movie updateMovie, int id) =>
    {
        var movie = await db.Movies.FindAsync(id);
        if (movie is null)
        {
            return Results.NotFound();
        }
        movie.Title = updateMovie.Title;
        movie.Genre = updateMovie.Genre;
        movie.Year = updateMovie.Year;
        await db.SaveChangesAsync();
        return Results.NoContent();
    };
}

static Func<MovieDb, int, Task<IResult>> DeleteMovie()
{
    return async (MovieDb db, int id) =>
    {
        var movie = await db.Movies.FindAsync(id);
        if (movie is null)
        {
            return Results.NotFound();
        }
        db.Movies.Remove(movie);
        return Results.Ok();
    };
}