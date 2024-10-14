using Movies.Application.Models;

namespace Movies.Application.Repository;

public class MovieRepository : IMovieRepository
{
	private readonly List<Movie> movies = [];

	public Task<bool> CreateAsync(Movie movie)
	{
		movies.Add(movie);
		return Task.FromResult(true);
	}

	public Task<bool> DeleteByIdAsync(Guid id)
	{
		var count = movies.RemoveAll(x => x.Id == id);
		return Task.FromResult(count > 0);
	}

	public Task<IEnumerable<Movie>> GetAllAsync()
	{
		return Task.FromResult(movies.AsEnumerable());
	}

	public Task<Movie?> GetByIdAsync(Guid id)
	{
		var movie = movies.SingleOrDefault(x => x.Id == id);
		return Task.FromResult(movie);
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