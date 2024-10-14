using Microsoft.AspNetCore.Mvc;
using Movies.Application.Models;
using Movies.Application.Repository;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers
{
	[ApiController]
	public class MoviesController : ControllerBase
	{
		private readonly IMovieRepository _movieRepository;

		public MoviesController(IMovieRepository movieRepository)
		{
			_movieRepository = movieRepository;
		}

		[HttpPost(ApiEndpoints.Movies.Create)]
		public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
		{
			var movie = new Movie
			{
				Id = Guid.NewGuid(),
				Genres = request.Genres.ToList(),
				Title = request.Title,
				YearOfRelease = request.YearOfRelease
			};
			var res = await _movieRepository.CreateAsync(movie);
			return CreatedAtAction(nameof(Get), new { id = movie.Id }, request);
		}

		[HttpGet(ApiEndpoints.Movies.GetALl)]
		public async Task<IActionResult> GetAll()
		{
			var movies = await _movieRepository.GetAllAsync();
			return Ok(movies);
		}

		[HttpGet(ApiEndpoints.Movies.Get)]
		public async Task<IActionResult> Get([FromRoute] Guid id)
		{
			var movie = await _movieRepository.GetByIdAsync(id);
			if (movie is null)
			{
				return NotFound();
			}
			return Ok(new MovieResponse
			{
				Id = movie.Id,
				Genres = movie.Genres,
				Title = movie.Title,
				Slug = movie.Slug,
				YearOfRelease = movie.YearOfRelease
			});
		}

		[HttpPut(ApiEndpoints.Movies.Update)]
		public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request)
		{
			var movie = new Movie
			{
				Id = id,
				Title = request.Title,
				Genres = request.Genres.ToList(),
				YearOfRelease = request.YearOfRelease
			};
			var res = await _movieRepository.UpdateMovieAsync(movie);
			if (!res)
			{
				return NotFound();
			}
			return Ok(new MovieResponse
			{
				Id = movie.Id,
				Genres = movie.Genres,
				Title = movie.Title,
				Slug= movie.Slug,
				YearOfRelease = movie.YearOfRelease
			});
		}

		[HttpDelete(ApiEndpoints.Movies.Delete)]
		public async Task<IActionResult> Delete([FromRoute] Guid id)
		{
			var res = await _movieRepository.DeleteByIdAsync(id);
			if (!res)
			{
				return NotFound();
			}
			return Ok();
		}

	}
}
