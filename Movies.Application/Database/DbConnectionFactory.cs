using Npgsql;
using System.Data;

namespace Movies.Application.Database;

public interface IDbConnectionFactory
{
	Task<IDbConnection> CreateConnationAsync();
}

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
	private readonly string _connectionString = connectionString;

	public async Task<IDbConnection> CreateConnationAsync()
	{
		var connection = new NpgsqlConnection(_connectionString);
		await connection.OpenAsync();
		return connection;
	}
}
