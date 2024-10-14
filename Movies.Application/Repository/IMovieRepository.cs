using Movies.Application.Models;

namespace Movies.Application.Repository;

public interface IMovieRepository
{
    Task<bool> CreateAsync(Movie movie);
    Task<Movie?> GetByIdAsync(Guid id);
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<bool> UpdateMovieAsync(Movie movie);
    Task<bool> DeleteByIdAsync(Guid id);
}