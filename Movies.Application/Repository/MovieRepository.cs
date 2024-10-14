using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;

namespace Movies.Application.Repository;

public class MovieRepository(IDbConnectionFactory dbConnectionFactory) : IMovieRepository
{
	private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;
	private readonly List<Movie> movies = [];

	public async Task<bool> CreateAsync(Movie movie)
	{
		var connection = await _dbConnectionFactory.CreateConnationAsync();
		var transaction = connection.BeginTransaction();
		var result = await connection.ExecuteAsync(new CommandDefinition("""
						insert into movies (id, slug, title,yearOfRelease)
						values (@Id,@Slug,@Title,@YearOfRelease)
						""", movie));

		if (result > 0)
		{
			foreach (var genre in movie.Genres)
			{
				await connection.ExecuteAsync(new CommandDefinition("""
					insert into genres (movieId, name)
					values (@MovieId,@Name)
					""", new { MovieId = movie.Id, Name = genre }));
			}
			transaction.Commit();
		}
		return result > 0;
	}

	public Task<bool> DeleteByIdAsync(Guid id)
	{
		var count = movies.RemoveAll(x => x.Id == id);
		return Task.FromResult(count > 0);
	}

	public async Task<IEnumerable<Movie>> GetAllAsync()
	{
		var connection = await _dbConnectionFactory.CreateConnationAsync();
		var result = await connection.QueryAsync(new CommandDefinition("""
				SELECT m.*,
					string_agg(g.name,  ',') as genres
				FROM movies m
					LEFT JOIN genres g
						ON g.movieId = m.id
				GROUP BY id
			"""));
		return result.Select(x => new Movie
		{
			Id = x.id,
			Title = x.title,
			YearOfRelease = x.yearofrelease,
			Genres = Enumerable.ToList(x.genres?.Split(','))
		});
	}

	public async Task<Movie?> GetByIdAsync(Guid id)
	{
		var connection = await _dbConnectionFactory.CreateConnationAsync();
		var movie = await connection.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition(
							"""SELECT * FROM Movies WHERE Id = @id""",
									new { id }));
		if (movie is null)
		{
			return null;
		}

		var genres = await connection.QueryAsync<string>(new CommandDefinition(
							"select name from genres where movieId = @id",
									new { id }));


		foreach (var genre in genres!)
		{
			movie.Genres.Add(genre);
		}
		return movie;
	}

	public Task<bool> UpdateMovieAsync(Movie movie)
	{
		var index = movies.FindIndex(x => x.Id.ToString() == movie.Id.ToString());
		if (index != -1)
		{
			movies[index] = movie;
			return Task.FromResult(true);
		}
		return Task.FromResult(false);
	}
}