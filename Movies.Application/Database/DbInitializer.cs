using Dapper;

namespace Movies.Application.Database;

public class DbInitializer(IDbConnectionFactory connectionFactory)
{
	private readonly IDbConnectionFactory _connectionFactory = connectionFactory;

	public async Task InitializeAsync()
	{
		using var connection = await _connectionFactory.CreateConnationAsync();


		await connection.ExecuteAsync("""
			create table if not exists movies (
			id UUID primary key,
			slug TEXT not null,
			title TEXT not null,
			yearOfRelease integer not null);
		""");

		await connection.ExecuteAsync("""
			create table if not exists Genres (
			movieId UUID references movies(id),
			name Text not null);
		""");
	}
}
